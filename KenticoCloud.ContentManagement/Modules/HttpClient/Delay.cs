using System;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class Delay: IDelay
    {
        public async Task DelayByMs(TimeSpan delay)
        {
            await Task.Delay(delay);
        }
    }
}
