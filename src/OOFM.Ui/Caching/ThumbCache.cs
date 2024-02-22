using System.IO;
using System.Net.Http;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OOFM.Ui.Caching;
internal class ThumbCache : CacheBase<string, ImageSource>
{
    private readonly HttpClient _http;

    public ThumbCache(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        PersistanceTime = TimeSpan.FromHours(1);
    }

    protected async override Task<ImageSource> ObtainValue(string url, CancellationToken cancellationToken = default)
    {
        var data = await _http.GetByteArrayAsync(url, cancellationToken);

        using (var ms = new MemoryStream(data))
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }
}
