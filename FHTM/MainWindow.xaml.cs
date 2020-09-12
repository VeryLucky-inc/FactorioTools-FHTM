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
            InitializeLocalization();
            InitializeValues();
            //TestDownloader();
            SearchGame();
            SearchMods();
        }

        #region Функции
        #region Перевод
        #region Basic 110920 2022
        public void InitializeLocalization(string name = null)
        {
            TranslateMods = "Моды";
            TranslateGame = "Игра";
            TranslateScenario = "Сценарии";
            TranslateProfile = "Профиль";
            TranslateSaves = "Сохранения";
            TranslateDownloads = "Загрузки";
            TranslateBuilds = "Сборки";
            TranslateBlueprints = "Чертежи";
            TranslateAbout = "О программе";
            TranslateAppSetings = "Настройки приложения:";
            TranslateAppTheme = "Тема приложения:";
            TranslateAppMode = "Режим: ";
            TranslateAppConsole = "Консоль:";
            TranslateModsPath = "Путь до модов";
            TranslateGamePath = "Путь до игры";
            TranslateAppPath = "Путь до FHTM";
            TranslateLocalization = "Локализация";
        }
        #endregion

        #endregion

        #region Initialize
        private void InitializeValues()
        {
            DownloadsList = new ObservableCollection<Downloader.FileDownload>();
        }
        private void InitializeProfile()
        {

        }
        #endregion

        #region Game Search
        private void SearchGame()
        {
            RegistryKey key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

                Console.AppendText($"Поиск установленной игры..." + "\n");
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
            }
            catch (Exception ex)
            {
                Console.AppendText($"{ex.Message}" + "\n");
            }
        }
        private void SearchMods()
        {
            string linkPathName = PathGame + "mods.lnk"; // Change this to the shortcut path
            
            if (System.IO.File.Exists(linkPathName))
            {
                //WshShellClass shell = new WshShellClass();
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(linkPathName); //Link the interface to our shortcut

                PathMods = link.TargetPath;
            }
        }
        #endregion

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

        #region Динамические переменные
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #region Для перевода
        #region Basic 110920 2022
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

        #endregion

        #endregion

        #region Другие данные

        private ObservableCollection<Downloader.FileDownload> _DownloadsList;
        public ObservableCollection<Downloader.FileDownload> DownloadsList
        {
            get { return _DownloadsList; }
            set { _DownloadsList = value; OnPropertyChanged("DownloadsList"); }
        }

        private string _PathMods;
        public string PathMods
        {
            get { return _PathMods; }
            set { _PathMods = value; OnPropertyChanged("PathMods"); }
        }

        private string _PathGame;
        public string PathGame
        {
            get { return _PathGame; }
            set { _PathGame = value; OnPropertyChanged("PathGame"); }
        }

        private string _PathApp;
        public string PathApp
        {
            get { return _PathApp; }
            set { _PathApp = value; OnPropertyChanged("PathApp"); }
        }

        private bool _ModeBuilds;
        public bool ModeBuilds
        {
            get { return _ModeBuilds; }
            set { _ModeBuilds = value; OnPropertyChanged("ModeBuilds"); }
        }

        #endregion

        #endregion
    }
}
