# Software.IP.Trace
> Use [README Syntax](https://www.makeareadme.com) to fill this document.

## Summary
- **Software.IP.Trace** is a WPF desktop application backed by [LiteDB](https://www.litedb.org) database.
- Application should be able to store geolocation data in the database, based on IP address.
- You can use [ipstack](https://ipstack.com) to get geolocation data. 
- The application should be able to add, delete or provide geolocation data on the base of IP address.

## License
> This project is under [MIT License](https://choosealicense.com/licenses/mit).

## Application specification
- WPF as frontend technology.
- Use [ipstack](https://ipstack.com) for the geolocation of IP addresses and URLs.
- The application can be built in .NET framework.
- Usage of any free library which will help implement solution is acceptable (e.g. Google material design, WiX, any MVVM library).
- The solution should also include base specs/tests coverage.

## Notes:
- Application is going to be tested on other local machines. 
- Solution should provide a quick and easy way to get the system up and running, including test data.
- Test behavior of the system under various "unfortunate" conditions (hint: How will the app behave when we take down the DB? How about the IPStack API?).

# Brainstorm
> **28.08.2022**
> 1. Create solution with all related projects.
> 2. .NET 6 selected as main framework.
> 3. Caliburn.Micro selected for MVVM.
> 4. Ninject selected for IoC.
> 5. Material.Design selected for theme.
> 6. LiteDB as local database (NoSQL).