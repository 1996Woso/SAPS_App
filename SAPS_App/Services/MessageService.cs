
namespace SAPS_App.Services
{
    public class MessageService : IMessageService
    {
        public string? ErrorMessage { get; private set; }

        public string? InfoMessage { get; private set; }

        public string? SuccessMessage { get; private set; }

        public bool IsMessageDisplayed { get; private set; }

        public bool IsFormDisplayed { get; private set; } = true;

        public event Action? Onchange;

        public void Clear()
        {
            ErrorMessage = "";
            SuccessMessage = "";
            InfoMessage = "";
            IsMessageDisplayed = false;
            IsFormDisplayed = true;
            NotifyStateChanged();
            
        }

        public async Task HideAsync(int delay = 5000)
        {
            await Task.Delay(delay);
            Clear();
        }

        public void Show(string? error, string? info, string? success)
        {
            ErrorMessage = error;
            InfoMessage = info;
            SuccessMessage = success;
            IsMessageDisplayed = true;
            IsFormDisplayed= false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => Onchange?.Invoke();
    }
}
