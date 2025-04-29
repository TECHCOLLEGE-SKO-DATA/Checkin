using CheckinLib.Platform;
using CheckinLib.ViewModels.UserControls;
namespace CheckinLib.ViewModels.Windows;

public class FakeNFCWindowViewModel : ViewModelBase
{
    FakeNFCViewModel _fakeNFCViewModel;
    public FakeNFCViewModel FakeNFCViewModel { get => _fakeNFCViewModel; set => _fakeNFCViewModel = value; }

    public FakeNFCWindowViewModel(IPlatform platform) : base(platform)
    {
        _fakeNFCViewModel = new FakeNFCViewModel(platform);
    }
}