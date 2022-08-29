using Ninject;

namespace Software.IP.Trace.IoC {
    public static class KernelLoadExtensions {
        public static void LoadIpTraceIoC(this IKernel kernel) =>
            kernel.Load(typeof(KernelLoadExtensions).Assembly);
    }
}
