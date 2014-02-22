using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XPCK_Template_Helper.Model;

namespace XPCK_Template_Helper.ViewModels
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string appName = "App Name", azureServiceAddress = "https://xplatformcloudkit.azure-mobile.net/", azureApplicationKey = "UYZnUrrabofKBELSRdRsmCGboyDGMJ15", privacyPolicy = "http://windotnet.blogspot.com/2013/11/app-privacy-policy.html", logoLoc = "", win8WallLoc = "", phone8WallLoc = "", appIdPhone8 = "test_client", adIdPhone8 = "Image480_80", appIdWin8 = "d25517cb-12d4-4699-8bdc-52040c712cab", adIdWin8 = "10042999";
        private int interval = 60, trialPeriod = 7, promoRuns=3;
        private Boolean enableAzure = false, enableRSS = false, enableLocal = false, initialSchema = true, enableTrial = false, simulateTrial = false, fullScreen = false, autoPlay = true, disableHyerplinks = false, hyperlinksNewTab = true, lightTheme = true, win8Wallpaper = false, phone8Wallpaper = false, enablePromo = true, enablePhone8Ads = false, hideAdsPurchasePhone8 = false, enableWin8Ads = false, hideAdsPurchaseWin8 = false;
        private Group selectedGroup = new Group("Microsoft", "http://reddit.com/r/Microsoft/.rss", false);
        private ObservableCollection<Group> groups = new ObservableCollection<Group>();
        private BackgroundWorker bw = new BackgroundWorker();

        public Settings()
        {
            Groups.Add(selectedGroup);
            bw.WorkerSupportsCancellation = false;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        #region BindingValues
        public Group SelectedGroup { get { return selectedGroup; } set { selectedGroup = value; RaisePropertyChanged("SelectedGroup"); } }

        public ObservableCollection<Group> Groups { get { return groups; } set { groups = value; RaisePropertyChanged("Groups"); } }

        public string AzureServiceAddress { get { return azureServiceAddress; } set { azureServiceAddress = value; } }

        public string AzureApplicationKey { get { return azureApplicationKey; } set { azureApplicationKey = value; } }

        public Boolean InitialSchema { get { return initialSchema; } set { initialSchema = value; } }

        public Boolean EnableLocal { get { return enableLocal; } set { enableLocal = value; } }

        public Boolean EnableRSS { get { return enableRSS; } set { enableRSS = value; } }

        public Boolean EnableAzure { get { return enableAzure; } set { enableAzure = value; } }

        public Boolean SimulateTrial { get { return simulateTrial; } set { simulateTrial = value; } }

        public Boolean EnableTrial { get { return enableTrial; } set { enableTrial = value; } }
        public int TrialPeriod { get { return trialPeriod; } set { trialPeriod = value.Equals("") ? 0 : value; } }

        public Boolean AutoPlay { get { return autoPlay; } set { autoPlay = value; } }

        public Boolean FullScreen { get { return fullScreen; } set { fullScreen = value; } }
        public Boolean DisableHyerplinks { get { return disableHyerplinks; } set { disableHyerplinks = value; } }

        public Boolean HyperlinksNewTab { get { return hyperlinksNewTab; } set { hyperlinksNewTab = value; } }

        public Boolean LightTheme { get { return lightTheme; } set { lightTheme = value; } }
        public string PrivacyPolicy { get { return privacyPolicy; } set { privacyPolicy = value; } }

        public string AppName { get { return appName; } set { appName = value; } }
        public int Interval { get { return interval; } set { interval = value; } }

        public string LogoLoc { get { return logoLoc; } set { logoLoc = value; RaisePropertyChanged("LogoLoc"); } }

        public Boolean Win8Wallpaper { get { return win8Wallpaper; } set { win8Wallpaper = value; RaisePropertyChanged("Win8Wallpaper"); } }
        public Boolean Phone8Wallpaper { get { return phone8Wallpaper; } set { phone8Wallpaper = value; RaisePropertyChanged("Phone8Wallpaper"); } }

        public string Phone8WallLoc { get { return phone8WallLoc; } set { phone8WallLoc = value; RaisePropertyChanged("Phone8WallLoc"); } }
        public string Win8WallLoc { get { return win8WallLoc; } set { win8WallLoc = value; RaisePropertyChanged("Win8WallLoc"); } }


        public Boolean EnablePromo { get { return enablePromo; } set { enablePromo = value; RaisePropertyChanged("EnablePromo"); } }
        public int PromoRuns { get { return promoRuns; } set { promoRuns = value; } }

        public Boolean EnablePhone8Ads { get { return enablePhone8Ads; } set { enablePhone8Ads = value; RaisePropertyChanged("EnablePhone8Ads"); } }
        public Boolean HideAdsPurchasePhone8 { get { return hideAdsPurchasePhone8; } set { hideAdsPurchasePhone8 = value; RaisePropertyChanged("HideAdsPurchasePhone8"); } }
        public string AppIdPhone8 { get { return appIdPhone8; } set { appIdPhone8 = value; RaisePropertyChanged("AppIdPhone8"); } }
        public string AdIdPhone8 { get { return adIdPhone8; } set { adIdPhone8 = value; RaisePropertyChanged("AdIdPhone8"); } }

        public Boolean EnableWin8Ads { get { return enableWin8Ads; } set { enableWin8Ads = value; RaisePropertyChanged("EnableWin8Ads"); } }
        public Boolean HideAdsPurchaseWin8 { get { return hideAdsPurchaseWin8; } set { hideAdsPurchaseWin8 = value; RaisePropertyChanged("HideAdsPurchaseWin8"); } }
        public string AppIdWin8 { get { return appIdWin8; } set { appIdWin8 = value; RaisePropertyChanged("AppIdWin8"); } }
        public string AdIdWin8 { get { return adIdWin8; } set { adIdWin8 = value; RaisePropertyChanged("AdIdWin8"); } }
        #endregion

        #region Commands
        public RelayCommand AddGroup
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Groups.Add(new Group("New Group", "Group URL", false));
                    SelectedGroup = Groups[Groups.Count - 1];
                });
            }
        }

        public RelayCommand SaveGroup
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Group test = new Group(selectedGroup.Key, selectedGroup.URL, selectedGroup.EnableFullScreen);
                    Boolean add = true;
                    foreach (Group g in Groups)
                    {
                        if (g.CompareTo(test) == 0) add = false;
                    }
                    if (add)
                        Groups.Add(test);
                });
            }
        }

        public RelayCommand RemoveGroup
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        foreach (Group g in Groups)
                        {
                            if (g.CompareTo(selectedGroup) == 0) Groups.Remove(g);
                        }
                    }
                    catch { }
                });
            }
        }

        public RelayCommand BrowseForLogo
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.Filter = "Images (.png, .jpg, .gif)|*.png;*.gif;*.jpg;*.jpeg";
                        Nullable<bool> result = dlg.ShowDialog();
                        if (result == true)
                        {
                            LogoLoc = dlg.FileName;
                        }
                        else
                        {
                            LogoLoc = "Please select file";
                        }
                    }
                    catch { }
                });
            }
        }

        public RelayCommand BrowseForWin8Wallpaper
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.Filter = "Images (.png, .jpg, .gif)|*.png;*.gif;*.jpg;*.jpeg";
                        Nullable<bool> result = dlg.ShowDialog();
                        if (result == true)
                        {
                            Win8WallLoc = dlg.FileName;
                        }
                        else
                        {
                            Win8WallLoc = "Please select file";
                        }
                    }
                    catch { }
                });
            }
        }

        public RelayCommand BrowseForPhone8Wallpaper
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.Filter = "Images (.png, .jpg, .gif)|*.png;*.gif;*.jpg;*.jpeg";
                        Nullable<bool> result = dlg.ShowDialog();
                        if (result == true)
                        {
                            Phone8WallLoc = dlg.FileName;
                        }
                        else
                        {
                            Phone8WallLoc = "Please select file";
                        }
                    }
                    catch { }
                });
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        public RelayCommand SaveApp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (!bw.IsBusy)
                        {

                            bw.RunWorkerAsync();
                        }
                    }
                    catch { }
                });
            }
        }
        #endregion

        #region workers
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowDataEntry = true;
        }

        private int totalFiles = 0, finishedFiles = 0;
        private bool showDataEntry = true;

        public bool ShowDataEntry { get { return showDataEntry; } set { showDataEntry = value; RaisePropertyChanged("ShowDataEntry"); } }

        public int FinishedFiles { get { return finishedFiles; } set { finishedFiles = value; RaisePropertyChanged("FinishedFiles"); } }
        public int TotalFiles { get { return totalFiles; } set { totalFiles = value; RaisePropertyChanged("TotalFiles"); } }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowDataEntry = false;
            FinishedFiles = 0;
            TotalFiles = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + "/XPlatformCloudKit", "*.*", SearchOption.AllDirectories).Count() + 14;
            CopyFolder(System.AppDomain.CurrentDomain.BaseDirectory + "/XPlatformCloudKit", System.AppDomain.CurrentDomain.BaseDirectory + "/" + AppName);
            if (!logoLoc.Equals(""))
                CopyLogos();
            if (phone8Wallpaper == true || win8Wallpaper == true)
                CopyWallpapers();
        }
        #endregion

        #region App Generator Methods
        public void CopyFolder(string source, string destination)
        {
            try
            {
                if (Directory.Exists(destination))
                    Directory.Delete(destination, true);
            }
            catch (Exception) { }
            try
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);
                string[] files = Directory.GetFiles(source);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destination, name);
                    if (name.Equals("AppSettings.cs"))
                        CopyAppSettings(file, dest);
                    else if (name.Equals("WMAppManifest.xml"))
                        CopyPhoneManifest(file, dest);
                    else if (name.Equals("Package.appxmanifest"))
                        CopyWindows8Manifest(file, dest);
                    else
                        File.Copy(file, dest);

                    FinishedFiles++;
                }
                string[] folders = Directory.GetDirectories(source);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destination, name);
                    CopyFolder(folder, dest);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error copying directory.\nPlease make sure the folder \"XPlatformCloudKit\" is in the same folder as this program.");
            }
        }

        private void CopyAppSettings(string file, string dest)
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            string content = "", FullScreenGroups = "";

            bool inRssAddress = false, inGroupsFullScreen = false;
            foreach (string line in lines)
            {
                String[] parts;
                if (line.Contains("string ApplicationName"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + appName + "\";\n";
                }
                else if (line.Contains("int CacheIntervalInMinutes"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + interval + ";\n";
                }
                else if (line.Contains("bool EnableAzureMobileService"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableAzure ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool CreateInitialSchemaForAzureMobileService"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableAzure ? initialSchema ? "true" : "false" : "false") + ";\n";
                }
                else if (line.Contains("string MobileServiceAddress"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + azureServiceAddress + "\";\n";
                }
                else if (line.Contains("string MobileServiceApplicationKey"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + azureApplicationKey + "\";\n";
                }
                else if (line.Contains("bool EnableRssService"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableRSS ? "true" : "false") + ";\n";
                }
                else if (line.Contains("RssSource[] RssAddressCollection"))
                {
                    inRssAddress = true;
                    content += line + "\n";
                }
                else if (inRssAddress)
                {
                    if (line.Contains("};"))
                    {
                        content += "{\n";
                        inRssAddress = false;
                        foreach (Group group in groups)
                        {
                            content += "new RssSource{Url = \"" + group.URL + "\", Group = \"" + group.Key + "\"},\n";
                            if (group.EnableFullScreen)
                            {
                                FullScreenGroups += "\"" + group.Key + "\", ";
                            }
                        }
                        content += "};\n";
                    }
                }
                else if (line.Contains("bool EnableLocalItemsFileService"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableLocal ? "true" : "false") + ";\n";
                }
                else if (line.Contains("string[] GroupsToDisplayInFullScreen"))
                {
                    inGroupsFullScreen = true;
                    content += line + "\n";
                }
                else if (inGroupsFullScreen)
                {
                    if (line.Contains("};"))
                    {
                        inGroupsFullScreen = false;
                        content += "{\n" + FullScreenGroups + "\n};\n";
                    }
                }
                else if (line.Contains("bool UseLightThemeForWindows8"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (lightTheme ? "true" : "false") + ";\n";
                }
                else if (line.Contains("string PrivacyPolicyUrl"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + privacyPolicy + "\";\n";
                }
                else if (line.Contains("bool TrialModeEnabled"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableTrial ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool SimulateTrialMode"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (simulateTrial ? "true" : "false") + ";\n";
                }
                else if (line.Contains("int TrialPeriodInDays"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + trialPeriod + ";\n";
                }
                else if (line.Contains("bool ForceYoutubeVideosToLoadFullScreen"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (fullScreen ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool AutoPlayYoutubeVideos"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (autoPlay ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool DisableHyperLinksInItemDescriptionView"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (disableHyerplinks ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool DisableOpeningHyperLinksInNewTab"))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (hyperlinksNewTab ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool EnablePhone8Background "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (phone8Wallpaper ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool EnableWin8Background "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (win8Wallpaper ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool EnableAppPromoRatingReminder "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enablePromo ? "true" : "false") + ";\n";
                }
                else if (line.Contains("int NumberOfRunsBeforeRateReminder "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + promoRuns + ";\n";
                }
                else if (line.Contains("bool EnablePubcenterAdsPhone8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enablePhone8Ads ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool HideAdsIfPurchasedPhone8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (hideAdsPurchasePhone8 ? "true" : "false") + ";\n";
                }
                else if (line.Contains("string PubcenterApplicationIdPhone8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + appIdPhone8 + "\";\n";
                }
                else if (line.Contains("string PubcenterAdUnitIdPhone8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + adIdPhone8 + "\";\n";
                }
                else if (line.Contains("bool EnablePubcenterAdsWin8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (enableWin8Ads ? "true" : "false") + ";\n";
                }
                else if (line.Contains("bool HideAdsIfPurchasedWin8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= " + (hideAdsPurchaseWin8 ? "true" : "false") + ";\n";
                }
                else if (line.Contains("string PubcenterApplicationIdWin8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + appIdWin8 + "\";\n";
                }
                else if (line.Contains("string PubcenterAdUnitIdWin8 "))
                {
                    parts = line.Split('=');
                    content += parts[0] + "= \"" + adIdWin8 + "\";\n";
                }
                else
                    content += line + "\n";
            }
            System.IO.File.WriteAllText(dest, content);
        }

        private void CopyPhoneManifest(string file, string dest)
        {
            string manifest = System.IO.File.ReadAllText(file);
            manifest = manifest.Replace("Title=\"XPlatformCloudKit.Phone8\"", "Title=\"" + appName + "\"");
            System.IO.File.WriteAllText(dest, manifest);
        }

        private void CopyWindows8Manifest(string file, string dest)
        {
            string manifest = System.IO.File.ReadAllText(file);
            manifest = manifest.Replace("<DisplayName>XPlatformCloudKit.Win8</DisplayName>\r\n", "<DisplayName>" + appName + "</DisplayName>\r\n");
            manifest = manifest.Replace("DisplayName=\"XPlatformCloudKit.Win8\"", "DisplayName=\"" + appName + "\"");
            System.IO.File.WriteAllText(dest, manifest);
        }

        private void CopyWallpapers()
        {
            string DIR = System.AppDomain.CurrentDomain.BaseDirectory + "/" + AppName;
            string file = "/XPlatformCloudKit.Phone8/Assets/Wallpaper.png";

            if (phone8Wallpaper == true)
            {
                try
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromFile(phone8WallLoc);
                    originalImage.Save(DIR+file, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch { MessageBox.Show("Error copying Phone 8 Wallpaper. Make sure you have selected an image."); }
            }
            FinishedFiles++;
            file = "/XPlatformCloudKit.Win8/Assets/Wallpaper.png";
            if (win8Wallpaper == true)
            {
                try
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromFile(win8WallLoc);
                    originalImage.Save(DIR+file, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (Exception e) { 
                    MessageBox.Show("Error copying Win 8 Wallpaper. Make sure you have selected an image."); 
                }
            }
            FinishedFiles++;
        }

        private void CopyLogos()
        {
            string DIR = System.AppDomain.CurrentDomain.BaseDirectory + "/" + AppName;
            string folder = "/XPlatformCloudKit.Phone8/Assets/";
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(logoLoc);
            System.Drawing.Bitmap resizedImage;
            string errMsg = "", currentFile = "";

            try
            {
                currentFile = DIR + folder + "ApplicationIcon.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(100, 100));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "300.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(300, 300));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            folder = "/XPlatformCloudKit.Phone8/Assets/Tiles/";
            try
            {
                currentFile = DIR + folder + "FlipCycleTileMedium.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(336, 336));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "FlipCycleTileLarge.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(691, 336));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "FlipCycleTileSmall.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(159, 159));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "IconicTileMediumLarge.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(134, 202));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "IconicTileSmall.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(71, 110));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }


            FinishedFiles++;
            folder = "/XPlatformCloudKit.Win8/Assets/";
            try
            {
                currentFile = DIR + folder + "Logo.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(150, 150));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "SmallLogo.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(30, 30));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "SplashScreen.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(620, 300));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "StoreLogo.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(50, 50));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;
            try
            {
                currentFile = DIR + folder + "WideLogo.png";
                System.IO.File.Delete(currentFile);
                resizedImage = new System.Drawing.Bitmap(originalImage, new System.Drawing.Size(310, 150));
                resizedImage.Save(currentFile, System.Drawing.Imaging.ImageFormat.Png);
                resizedImage.Dispose();
            }
            catch { errMsg += currentFile + "\n"; }
            FinishedFiles++;

            if (!errMsg.Equals("")) MessageBox.Show("Encountered Errors Copying Following Images:\n" + errMsg);
        }
        #endregion
    }
}

