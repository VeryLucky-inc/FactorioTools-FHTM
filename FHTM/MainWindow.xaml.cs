using System;
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
            TestDownloader();
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
            TranslateConsole = "Консоль:";
            TranslateModsPath = "Путь до модов";
            TranslateGamePath = "Путь до игры";
            TranslateAppPath = "Путь до FHTM";
            TranslateLocalization = "Локализация";
        }
        #endregion

        #endregion

        #region Тестовые функции
        private void TestDownloaderInterface()
        {
            DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Krastorio2/1.0.4.zip", ".\\"));
            DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Nanobots/3.2.8.zip", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
            //DownloadsList.Add(new Downloader.FileDownload("", ".\\"));
        }
        public void TestDownloader()
        {
            DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Krastorio2/1.0.4.zip", ".\\"));
            DownloadsList.Add(new Downloader.FileDownload("https://factorio-launcher-mods.storage.googleapis.com/Nanobots/3.2.8.zip", ".\\"));
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

        private string _TranslateConsole;
        public string TranslateConsole
        {
            get { return _TranslateConsole; }
            set { _TranslateConsole = value; OnPropertyChanged("TranslateConsole"); }
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
        #endregion

        #endregion

        #region Другие данные

        private ObservableCollection<Downloader.FileDownload> _DownloadsList = new ObservableCollection<Downloader.FileDownload>();
        public ObservableCollection<Downloader.FileDownload> DownloadsList
        {
            get { return _DownloadsList; }
            set { _DownloadsList = value; OnPropertyChanged("DownloadsList"); }
        }

        #endregion

        #endregion
    }
}
