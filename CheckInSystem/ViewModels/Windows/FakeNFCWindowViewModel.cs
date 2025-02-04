using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
namespace CheckInSystem.ViewModels.Windows;

public class FakeNFCWindowViewModel : ViewModelBase
{
    FakeNFCViewModel _fakeNFCViewModel;
    public FakeNFCViewModel FakeNFCViewModel { get => _fakeNFCViewModel; set => _fakeNFCViewModel = value; }

    public FakeNFCWindowViewModel(IPlatform platform) : base(platform)
    {
        _fakeNFCViewModel = new FakeNFCViewModel(platform);
    }
}