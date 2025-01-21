using System.IO;
using System.Windows;

namespace CheckInSystem.ViewModels.Windows;

public class EmployeeOverviewViewModel : ViewmodelBase
{
    private string ConfigFilePath = "";
    private decimal _scaleSize = 1.0M;
    private ResizeMode _resizeMode = ResizeMode.NoResize;
    private WindowStyle _windowStyle = WindowStyle.None;
    WindowState _windowState;

    public decimal ScaleSize
    {
        get => _scaleSize;
        set => SetProperty(ref _scaleSize, value);
    }

    public ResizeMode ResizeMode
    {
        get => _resizeMode;
        set => SetProperty(ref _resizeMode, value);
    }
    public WindowStyle WindowStyle
    {
        get => _windowStyle;
        set => SetProperty(ref _windowStyle, value);
    }
    public WindowState WindowState
    {
        get => _windowState;
        set => SetProperty(ref _windowState, value);
    }

    public void ZoomIn()
    {
        ScaleSize += 0.1M;
    }

    public void ZoomOut()
    {
        ScaleSize -= 0.1M;
    }

    public void ToggleFullscreen()
    {
        if (ResizeMode == ResizeMode.NoResize) {
            ResizeMode = ResizeMode.CanResizeWithGrip;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
        }
        else
        {
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
        }
    }

    public EmployeeOverviewViewModel()
    {
        string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath += @"\EmployeeOverviewViewModelConfig.txt";
        ConfigFilePath = filePath;
        ReadConfig();
    }

    //TODO: consider moving Readconfig() and UpdateConfig to to a config class and use a propper saving format
    private void ReadConfig()
    {
        if (!File.Exists(ConfigFilePath))
        {
            File.WriteAllText(ConfigFilePath, ScaleSize.ToString());
            return;
        }

        try
        {
            string contents = File.ReadAllText(ConfigFilePath);
            ScaleSize = Convert.ToDecimal(contents);
        }
        catch
        {
            File.Delete(ConfigFilePath);
            UpdateConfig();
        }
    }

    public void UpdateConfig() 
    {
        File.WriteAllText(ConfigFilePath, ScaleSize.ToString());
    }
}