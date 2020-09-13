using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Management;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using ControlzEx.Theming;
using System.Diagnostics;
using System.Reflection;

namespace FHTM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeValues();
            #region Sync windows theme
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();
            #endregion
            InitializeProfile();

            //TestDownloader();


            AppLoaded = true;
        }

        #region Функции
        #region Перевод
        public void InitializeLocalization(string name = null)
        {
            if (!Directory.Exists(@".\i18n"))
            {
                Directory.CreateDirectory(@".\i18n");
                System.IO.File.WriteAllText($@".\i18n\Russian", Profile.Serialize.ToJson<Profile.Localization>(new Profile.Localization
                {
                    TranslateMods = "Моды",
                    TranslateGame = "Игра",
                    TranslateScenario = "Сценарии",
                    TranslateProfile = "Профиль",
                    TranslateSaves = "Сохранения",
                    TranslateDownloads = "Загрузки",
                    TranslateBuilds = "Сборки",
                    TranslateBlueprints = "Чертежи",
                    TranslateAbout = "О программе",
                    TranslateAppSetings = "Настройки приложения:",
                    TranslateAppTheme = "Тема приложения:",
                    TranslateAppMode = "Режим: ",
                    TranslateAppConsole = "Консоль:",
                    TranslateModsPath = "Путь до модов",
                    TranslateGamePath = "Путь до игры",
                    TranslateAppPath = "Открыть",
                    TranslateSavesPath = "Путь до сохр.",
                    TranslateGameConfigPath = "Настр. игры",
                    TranslateLocalization = "Локализация",
                    TranslateSearchInstalledGame = "Поиск установленной игры...",
                    TranslateDefault = "Обычный",
                    TranslateResetSettings = "Сбросить настройки программы",
                    TranslateResetSettingsDialogTitle = "Сброс настроек программы",
                    TranslateResetSettingsDialogText = "Внимание\nЭто сбросит все настройки программы!\nПродолжить?"
                }));
                System.IO.File.WriteAllText($@".\i18n\English", Profile.Serialize.ToJson<Profile.Localization>(new Profile.Localization
                {
                    TranslateMods = "Mods",
                    TranslateGame = "Game",
                    TranslateScenario = "Scenarios",
                    TranslateProfile = "Profile",
                    TranslateSaves = "Saves",
                    TranslateDownloads = "Downloads",
                    TranslateBuilds = "Builds",
                    TranslateBlueprints = "Blueprints",
                    TranslateAbout = "About app",
                    TranslateAppSetings = "App settings:",
                    TranslateAppTheme = "App theme:",
                    TranslateAppMode = "App mode: ",
                    TranslateAppConsole = "Console:",
                    TranslateModsPath = "Path to mods",
                    TranslateGamePath = "Path to game",
                    TranslateAppPath = "Open folder",
                    TranslateSavesPath = "Path to saves",
                    TranslateGameConfigPath = "Game settings",
                    TranslateLocalization = "Localization",
                    TranslateSearchInstalledGame = "Search installed game...",
                    TranslateDefault = "Default",
                    TranslateResetSettings = "Reset all app settings",
                    TranslateResetSettingsDialogTitle = "Reset app settings",
                    TranslateResetSettingsDialogText = "Attention\nThis will reset all program settings!\nDo you want to continue?"
                }));
            }
            foreach (string l in Directory.GetFiles(@".\i18n"))
            {
                if (!String.IsNullOrWhiteSpace(l)) LocalesList.Add(System.IO.Path.GetFileName(l));
            }
            LoadLocalozation();
        }
        public void LoadLocalozation()
        {
            Profile.Localization Locale = Profile.Serialize.FromJson<Profile.Localization>(System.IO.File.ReadAllText($@".\i18n\{AppLocalization}"));
            TranslateMods = Locale.TranslateMods;
            TranslateGame = Locale.TranslateGame;
            TranslateScenario = Locale.TranslateScenario;
            TranslateProfile = Locale.TranslateProfile;
            TranslateSaves = Locale.TranslateSaves;
            TranslateDownloads = Locale.TranslateDownloads;
            TranslateBuilds = Locale.TranslateBuilds;
            TranslateBlueprints = Locale.TranslateBlueprints;
            TranslateAbout = Locale.TranslateAbout;
            TranslateAppSetings = Locale.TranslateAppSetings;
            TranslateAppTheme = Locale.TranslateAppTheme;
            TranslateAppMode = Locale.TranslateAppMode;
            TranslateAppConsole = Locale.TranslateAppConsole;
            TranslateModsPath = Locale.TranslateModsPath;
            TranslateGamePath = Locale.TranslateGamePath;
            TranslateAppPath = Locale.TranslateAppPath;
            TranslateSavesPath = Locale.TranslateSavesPath;
            TranslateGameConfigPath = Locale.TranslateGameConfigPath;
            TranslateLocalization = Locale.TranslateLocalization;
            TranslateSearchInstalledGame = Locale.TranslateSearchInstalledGame;
            TranslateDefault = Locale.TranslateDefault;
            TranslateResetSettings = Locale.TranslateResetSettings;
            TranslateResetSettingsDialogTitle = Locale.TranslateResetSettingsDialogTitle;
            TranslateResetSettingsDialogText = Locale.TranslateResetSettingsDialogText;
        }
        #endregion

        #region Initialize
        private void InitializeValues()
        {
            DownloadsList = new ObservableCollection<Downloader.FileDownload>();
            LocalesList = new ObservableCollection<string>();
        }
        private void InitializeProfile()
        {
            PathApp = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(@".\Profile"))
            {
                if (System.IO.File.Exists(@".\Profile\Config.json"))
                {
                    LoadProfile();
                }
                else
                {
                    NewProfile();
                }
            }
            else
            {
                NewProfile();
            }

            ChangeTheme();

            return;

            void NewProfile()
            {
                switch (System.Globalization.CultureInfo.CurrentCulture.ToString())
                {
                    case "ru-RU":
                        AppLocalization = "Russian";
                        break;

                    case "ru-KZ":
                        AppLocalization = "Russian";
                        break;

                    case "en-US":
                        AppLocalization = "English";
                        break;

                    default:
                        AppLocalization = "English";
                        break;
                }
                InitializeLocalization();
                SearchGame();
                SaveProfile();
            }
        }

        private void SaveProfile()
        {
            if (!Directory.Exists(@".\Profile"))
            {
                Directory.CreateDirectory(@".\Profile");
            }
            System.IO.File.WriteAllText(@".\Profile\Config.json", Profile.Serialize.ToJson(new Profile.Config { AppTheme = AppTheme, AppLocalization = AppLocalization, ModeBuilds = ModeBuilds, Username = Username, Password = Password, PathGame = PathGame, PathGameConfig = PathGameConfig, PathMods = PathMods, PathSaves = PathSaves }));
        }
        private void LoadProfile()
        {
            Profile.Config Config = Profile.Serialize.FromJson<Profile.Config>(System.IO.File.ReadAllText(@".\Profile\Config.json"));
            AppTheme = Config.AppTheme;
            AppLocalization = Config.AppLocalization;
            ModeBuilds = Config.ModeBuilds;
            Username = Config.Username;
            Password = Config.Password;
            PathGame = Config.PathGame;
            PathGameConfig = Config.PathGameConfig;
            PathMods = Config.PathMods;
            PathSaves = Config.PathSaves;
            InitializeLocalization();
        }
        #endregion

        #region Game Search
        private void SearchGame()
        {
            RegistryKey key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

                Console.AppendText(TranslateSearchInstalledGame + "\n");
                foreach (String keyName in key.GetSubKeyNames())
                {
                    dynamic subkey = key.OpenSubKey(keyName);
                    string stn = subkey.GetValue("DisplayName") as string;
                    string stv = subkey.GetValue("InstallLocation") as string;

                    if (!String.IsNullOrWhiteSpace(stn) && !String.IsNullOrWhiteSpace(stv))
                    {
                        if (stn.Contains("Factorio") || stv.Contains("Factorio"))
                        {
                            Console.AppendText($"{stn}: {stv}" + "\n");
                            PathGame = stv;
                        }
                    }
                }

                SearchGameAdditionalFolders();
            }
            catch (Exception ex)
            {
                Console.AppendText($"Search game ERROR: {ex.ToString()}" + "\n");
            }
        }
        private void SearchGameAdditionalFolders()
        {
            try
            {
                if (Directory.Exists($"{PathGame}\\config")) PathGameConfig = $"{PathGame}config\\";
                else PathGameConfig = GetLnkInnerPath(PathGame + "config.lnk") + "\\";
                if (Directory.Exists($"{PathGame}\\mods")) PathMods = $"{PathGame}mods\\";
                else PathMods = GetLnkInnerPath(PathGame + "mods.lnk") + "\\";
                if (Directory.Exists($"{PathGame}\\saves")) PathSaves = $"{PathGame}saves\\";
                else PathSaves = GetLnkInnerPath(PathGame + "saves.lnk") + "\\";
            }
            catch (Exception ex)
            {
                Console.AppendText($"Search game other paths ERROR: {ex.ToString()}" + "\n");
            }
        }
        #endregion

        private string GetLnkInnerPath(string linkPathName)
        {
            if (System.IO.File.Exists(linkPathName))
            {
                //WshShellClass shell = new WshShellClass();
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(linkPathName); //Link the interface to our shortcut

                return link.TargetPath;
            }
            else return "";
        }
        private void ChangeTheme()
        {
            var theme = ThemeManager.Current.DetectTheme();
            ThemeManager.Current.ChangeTheme(this, new Theme($"Custom{AppTheme}Current", $"Custom{AppTheme}Current", AppTheme, theme.ColorScheme, theme.PrimaryAccentColor, theme.ShowcaseBrush, true, theme.IsHighContrast));
        }

        #region Тестовые функции
        private void TestDownloaderInterface()
        {
            //DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Krastorio2/1.0.4.zip", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Nanobots/3.2.8.zip", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
        }
        public void TestDownloader()
        {
            //DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Krastorio2/1.0.4.zip", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Nanobots/3.2.8.zip", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
        }
        #endregion

        #endregion

        #region Переменные
        private string _AppTheme = "Light";
        private string AppTheme { get { return _AppTheme; } set { _AppTheme = value; if (AppLoaded) SaveProfile(); } }
        private string TranslateSearchInstalledGame;
        private string _Password;
        private string Password { get { return _Password; } set { _Password = value; if (AppLoaded) SaveProfile(); } }
        private bool AppLoaded = false;
        #endregion

        #region Динамические переменные
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #region Для перевода
        private string _TranslateMods;
        public string TranslateMods
        {
            get { return _TranslateMods; }
            set { _TranslateMods = value; OnPropertyChanged("TranslateMods"); }
        }

        private string _TranslateGame;
        public string TranslateGame
        {
            get { return _TranslateGame; }
            set { _TranslateGame = value; OnPropertyChanged("TranslateGame"); }
        }

        private string _TranslateScenario;
        public string TranslateScenario
        {
            get { return _TranslateScenario; }
            set { _TranslateScenario = value; OnPropertyChanged("TranslateScenario"); }
        }

        private string _TranslateProfile;
        public string TranslateProfile
        {
            get { return _TranslateProfile; }
            set { _TranslateProfile = value; OnPropertyChanged("TranslateProfile"); }
        }

        private string _TranslateSaves;
        public string TranslateSaves
        {
            get { return _TranslateSaves; }
            set { _TranslateSaves = value; OnPropertyChanged("TranslateSaves"); }
        }

        private string _TranslateDownloads;
        public string TranslateDownloads
        {
            get { return _TranslateDownloads; }
            set { _TranslateDownloads = value; OnPropertyChanged("TranslateDownloads"); }
        }

        private string _TranslateBuilds;
        public string TranslateBuilds
        {
            get { return _TranslateBuilds; }
            set { _TranslateBuilds = value; OnPropertyChanged("TranslateBuilds"); }
        }

        private string _TranslateBlueprints;
        public string TranslateBlueprints
        {
            get { return _TranslateBlueprints; }
            set { _TranslateBlueprints = value; OnPropertyChanged("TranslateBlueprints"); }
        }

        private string _TranslateAbout;
        public string TranslateAbout
        {
            get { return _TranslateAbout; }
            set { _TranslateAbout = value; OnPropertyChanged("TranslateAbout"); }
        }

        private string _TranslateAppSetings;
        public string TranslateAppSetings
        {
            get { return _TranslateAppSetings; }
            set { _TranslateAppSetings = value; OnPropertyChanged("TranslateAppSetings"); }
        }

        private string _TranslateAppTheme;
        public string TranslateAppTheme
        {
            get { return _TranslateAppTheme; }
            set { _TranslateAppTheme = value; OnPropertyChanged("TranslateAppTheme"); }
        }

        private string _TranslateAppConsole;
        public string TranslateAppConsole
        {
            get { return _TranslateAppConsole; }
            set { _TranslateAppConsole = value; OnPropertyChanged("TranslateAppConsole"); }
        }

        private string _TranslateModsPath;
        public string TranslateModsPath
        {
            get { return _TranslateModsPath; }
            set { _TranslateModsPath = value; OnPropertyChanged("TranslateModsPath"); }
        }

        private string _TranslateGamePath;
        public string TranslateGamePath
        {
            get { return _TranslateGamePath; }
            set { _TranslateGamePath = value; OnPropertyChanged("TranslateGamePath"); }
        }

        private string _TranslateAppPath;
        public string TranslateAppPath
        {
            get { return _TranslateAppPath; }
            set { _TranslateAppPath = value; OnPropertyChanged("TranslateAppPath"); }
        }

        private string _TranslateLocalization;
        public string TranslateLocalization
        {
            get { return _TranslateLocalization; }
            set { _TranslateLocalization = value; OnPropertyChanged("TranslateLocalization"); }
        }

        private string _TranslateAppMode;
        public string TranslateAppMode
        {
            get { return _TranslateAppMode; }
            set { _TranslateAppMode = value; OnPropertyChanged("TranslateAppMode"); }
        }

        private string _TranslateSavesPath;
        public string TranslateSavesPath
        {
            get { return _TranslateSavesPath; }
            set { _TranslateSavesPath = value; OnPropertyChanged("TranslateSavesPath"); }
        }

        private string _TranslateGameConfigPath;
        public string TranslateGameConfigPath
        {
            get { return _TranslateGameConfigPath; }
            set { _TranslateGameConfigPath = value; OnPropertyChanged("TranslateGameConfigPath"); }
        }

        private string _TranslateDefault;
        public string TranslateDefault
        {
            get { return _TranslateDefault; }
            set { _TranslateDefault = value; OnPropertyChanged("TranslateDefault"); }
        }

        private string _TranslateResetSettings;
        public string TranslateResetSettings
        {
            get { return _TranslateResetSettings; }
            set { _TranslateResetSettings = value; OnPropertyChanged("TranslateResetSettings"); }
        }

        private string _TranslateResetSettingsDialogTitle;
        public string TranslateResetSettingsDialogTitle
        {
            get { return _TranslateResetSettingsDialogTitle; }
            set { _TranslateResetSettingsDialogTitle = value; OnPropertyChanged("TranslateResetSettingsDialogTitle"); }
        }

        private string _TranslateResetSettingsDialogText;
        public string TranslateResetSettingsDialogText
        {
            get { return _TranslateResetSettingsDialogText; }
            set { _TranslateResetSettingsDialogText = value; OnPropertyChanged("TranslateResetSettingsDialogText"); }
        }

        #endregion

        #region Другие данные

        private ObservableCollection<Downloader.FileDownload> _DownloadsList;
        public ObservableCollection<Downloader.FileDownload> DownloadsList
        {
            get { return _DownloadsList; }
            set { _DownloadsList = value; OnPropertyChanged("DownloadsList"); }
        }

        private ObservableCollection<string> _LocalesList;
        public ObservableCollection<string> LocalesList
        {
            get { return _LocalesList; }
            set { _LocalesList = value; OnPropertyChanged("LocalesList"); }
        }

        private string _AppLocalization;
        public string AppLocalization
        {
            get { return _AppLocalization; }
            set { _AppLocalization = value; OnPropertyChanged("AppLocalization"); if (AppLoaded) SaveProfile(); }
        }

        private string _PathMods;
        public string PathMods
        {
            get { return _PathMods; }
            set { _PathMods = value; OnPropertyChanged("PathMods"); if (AppLoaded) SaveProfile(); }
        }

        private string _PathGame;
        public string PathGame
        {
            get { return _PathGame; }
            set { _PathGame = value; OnPropertyChanged("PathGame"); if (AppLoaded) SaveProfile(); }
        }

        private string _PathApp;
        public string PathApp
        {
            get { return _PathApp; }
            set { _PathApp = value; OnPropertyChanged("PathApp"); if (AppLoaded) SaveProfile(); }
        }

        private string _PathSaves;
        public string PathSaves
        {
            get { return _PathSaves; }
            set { _PathSaves = value; OnPropertyChanged("PathSaves"); if (AppLoaded) SaveProfile(); }
        }

        private string _PathGameConfig;
        public string PathGameConfig
        {
            get { return _PathGameConfig; }
            set { _PathGameConfig = value; OnPropertyChanged("PathGameConfig"); if (AppLoaded) SaveProfile(); }
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; OnPropertyChanged("Username"); if (AppLoaded) SaveProfile(); }
        }

        private bool _ModeBuilds;
        public bool ModeBuilds
        {
            get { return _ModeBuilds; }
            set { _ModeBuilds = value; OnPropertyChanged("ModeBuilds"); if (AppLoaded) SaveProfile(); }
        }

        #endregion

        #endregion

        #region Events

        #region Theme changing
        private void DarkThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme = "Dark";
            ChangeTheme();
        }

        private void LightThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme = "Light";
            ChangeTheme();
        }
        #endregion

        private void SelectedLocaleChanged(object sender, EventArgs e)
        {
            LoadLocalozation();
            SaveProfile();
        }

        private void ChangePath(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.UseDescriptionForTitle = true;

            if (((Button)sender).Content.ToString() == TranslateModsPath)
            {
                if (Convert.ToBoolean(dialog.ShowDialog()))
                {
                    dialog.Description = TranslateMods;
                    PathMods = dialog.SelectedPath + "\\";
                }
            }
            else if (((Button)sender).Content.ToString() == TranslateGamePath)
            {
                if (Convert.ToBoolean(dialog.ShowDialog()))
                {
                    dialog.Description = TranslateGame;
                    PathGame = dialog.SelectedPath + "\\";
                    SearchGameAdditionalFolders();
                }
            }
            else if (((Button)sender).Content.ToString() == TranslateSavesPath)
            {
                if (Convert.ToBoolean(dialog.ShowDialog()))
                {
                    dialog.Description = TranslateSaves;
                    PathSaves = dialog.SelectedPath + "\\";
                }
            }
            else if (((Button)sender).Content.ToString() == TranslateGameConfigPath)
            {
                if (Convert.ToBoolean(dialog.ShowDialog()))
                {
                    dialog.Description = TranslateGameConfigPath;
                    PathGameConfig = dialog.SelectedPath + "\\";
                }
            }
            else if (((Button)sender).Content.ToString() == TranslateAppPath)
            {
                Process.Start(PathApp);
            }
        }

        private void ResetAppSettings(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(TranslateResetSettingsDialogText, TranslateResetSettingsDialogTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.ServiceNotification) == MessageBoxResult.Yes)
            {
                System.IO.File.Delete(@".\Profile\Config.json");
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Close();
            }
        }

        #endregion


    }
}
