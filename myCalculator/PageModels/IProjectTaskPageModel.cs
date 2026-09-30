using CommunityToolkit.Mvvm.Input;
using myCalculator.Models;

namespace myCalculator.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}