namespace SAPS_App.Services
{
    public interface IMessageService
    {
        string? ErrorMessage { get; }
        string? InfoMessage { get; }
        string? SuccessMessage { get; }
        bool IsMessageDisplayed { get; }
        bool IsFormDisplayed { get; }

        public event Action? Onchange;
        void Clear();
        void Show(string? error, string? info, string? success);
        Task HideAsync(int delay = 5000);
    }
}
