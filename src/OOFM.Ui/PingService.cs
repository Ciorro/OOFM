using Microsoft.Extensions.Hosting;
using OOFM.Core.Api;
using System.Net.Http;

namespace OOFM.Ui;

//GZD http connection keepalive
internal class PingService(IHttpClientProvider httpClientProvider) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var msg = new HttpRequestMessage(HttpMethod.Head, "https://open.fm/api"))
            {
                await httpClientProvider.GetHttpClient().SendAsync(msg);
            }

            await Task.Delay(30_000);
        }
    }
}
