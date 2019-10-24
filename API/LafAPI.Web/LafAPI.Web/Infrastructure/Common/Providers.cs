namespace LafAPI.Web.Infrastructure.Common
{
    public static class Providers
    {
        public static string NormalizedAPIPath(this string path, string contentRootPath) =>
            path.Replace(contentRootPath, string.Empty).Replace("\\", "/")
                .Replace("/wwwroot", string.Empty);

        public static string PathToKeys(this string environment) =>
            environment == ApplicationConstants.Development ? "Logging:" : string.Empty;

        public static string PathToJwtTokenValidationKey(this string environment) =>
            environment == ApplicationConstants.Development ? "Logging:JwtTokenValidation" : "JwtTokenValidation";

        public static string[] CreateDomainsByFourth32Bytes(
            int first32bytes = 192,
            int second32bytes = 168,
            int third32bytes = 0,
            int? port = null,
            bool withDefaultIPs = true,
            params int[] fourthBytes)
        {
            string HttpGen(int f, int s, int t, int fh, int? p = null, bool sr = false) =>
                $"http{(sr ? "s" : string.Empty)}://{f}.{s}.{t}.{fh}{(p.HasValue ? $":{p.Value}" : string.Empty)}";

            var index = 0;
            var length = (fourthBytes.Length * 2) + (withDefaultIPs ? 3 * 2 : 0);
            var ips = new string[length];
            if (withDefaultIPs)
            {
                ips[index++] = $"http://localhost:{port}";
                ips[index++] = $"https://localhost:{port}";
                ips[index++] = HttpGen(127, 0, 0, 1, port);
                ips[index++] = HttpGen(127, 0, 0, 1, port, true);
                ips[index++] = HttpGen(0, 0, 0, 0, port);
                ips[index++] = HttpGen(0, 0, 0, 0, port, true);
            }

            for (int i = 0; i < fourthBytes.Length; i++)
            {
                ips[i + index] = HttpGen(first32bytes, second32bytes, third32bytes, fourthBytes[i], port);
                ips[i + ++index] = HttpGen(first32bytes, second32bytes, third32bytes, fourthBytes[i], port, true);
            }

            return ips;
        }
    }
}