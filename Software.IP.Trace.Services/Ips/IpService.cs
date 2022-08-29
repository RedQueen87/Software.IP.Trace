using System.Diagnostics;
using LiteDB;
using Software.IP.Trace.Model;

namespace Software.IP.Trace.Services.Ips {
    public class IpService : IIpService {
        public Task<IpTraceDO[]> List() {
            return Task.Run(() => {
                using var db = GetDatabase();
                var source = db
                    .GetCollection<Coordinates>("coordinates")
                    .Query()
                    .Select(ip => new IpTraceDO {
                        Ip = ip.Ip,
                        Latitude = ip.Latitude,
                        Longitude = ip.Longitude
                    })
                    .ToArray();
                return source;
            });
        }

        public Task Add(string ip, double latitude, double longitude) {
            return Task.Run(() => {
                using var db = GetDatabase();
                var coordinatesCollection = db.GetCollection<Coordinates>("coordinates");
                var model = coordinatesCollection
                    .Query()
                    .Where(m => m.Ip == ip)
                    .SingleOrDefault();
                if (model == null) {
                    model = new Coordinates {
                        Ip = ip,
                        Latitude = latitude,
                        Longitude = longitude
                    };
                    coordinatesCollection.Insert(model);
                    return;
                }

                model.Latitude = latitude;
                model.Longitude = longitude;
                coordinatesCollection.Update(model);
            });
        }

        public Task Delete(string ip) {
            return Task.Run(() => {
                using var db = GetDatabase();
                var coordinatesCollection = db.GetCollection<Coordinates>("coordinates");
                coordinatesCollection.DeleteMany(coordinates => coordinates.Ip == ip);
            });
        }

        private LiteDatabase GetDatabase() {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            return new LiteDatabase(@$"{dir}\local.db");
        }
    }

    public interface IIpService {
        Task<IpTraceDO[]> List();

        Task Add(string ip, double latitude, double longitude);

        Task Delete(string ip);
    }


    public class IpTraceDO {
        public string Ip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
