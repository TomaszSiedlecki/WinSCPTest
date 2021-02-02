using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using System.Linq;

namespace WinSCP_Test
{
    [TestClass]
    public class UIWinSCP
    {
        private Application App;
        private readonly string appPath = @"C:\Program Files (x86)\WinSCP\WinSCP.exe";
        private const string APP_TITLE = "WinSCP";
        private int sleep = 2000;

    [TestMethod]
        public void DisableConnectionButtonTest()
        {
            App = Application.Launch(appPath);
            Thread.Sleep(sleep);
            Window window = App.GetWindow(APP_TITLE);
            //SPRAWDZENIE NAZWY APLIKACJI
            Assert.AreEqual(APP_TITLE, window.Title); 
            //SPRAWDZANIE PRZYCISKÓW BEZ AKTYWNEGO POŁĄCZNIA
            Button btnCatalog = window.Get<Button>(SearchCriteria.ByText("Porównaj katalogi"));
            Assert.IsFalse(btnCatalog.Enabled); 
           
            Button btnUpdate = window.Get<Button>(SearchCriteria.ByText("Automatycznie aktualizuj katalog zdalny"));
            Assert.IsFalse(btnUpdate.Enabled); 
           
            Button btnSync = window.Get<Button>(SearchCriteria.ByText("Synchronizuj"));
            Assert.IsFalse(btnSync.Enabled); 
            
            Button btnTerminal = window.Get<Button>(SearchCriteria.ByText("Otwórz terminal"));
            Assert.IsFalse(btnTerminal.Enabled); 
           
            Button btnOpenP = window.Get<Button>(SearchCriteria.ByText("Otwórz w PuTTY"));
            Assert.IsFalse(btnOpenP.Enabled); 
            
            Button btnSyncSearch = window.Get<Button>(SearchCriteria.ByText("Synchronizuj przeglądanie"));
            Assert.IsFalse(btnSyncSearch.Enabled);     
            
            Button btnDelete = window.Get<Button>(SearchCriteria.ByText("Usuń"));
            Assert.IsFalse(btnDelete.Enabled); 
            
            Button btnName = window.Get<Button>(SearchCriteria.ByText("Zmień nazwę"));
            Assert.IsFalse(btnName.Enabled); 
            
            Button btnMenu = window.Get<Button>(SearchCriteria.ByText("Właściwości"));
            Assert.IsFalse(btnMenu.Enabled); 
            //Zamykanie okna
            window.Close();
        }
        [TestMethod]
        public void NewConnectionTest()
        {
            App = Application.Launch(appPath);
            Thread.Sleep(sleep);
            Window window = App.GetWindow(APP_TITLE);
            //Uruchomienie logowania do sesji
            window.Keyboard.HoldKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.CONTROL);
            window.Keyboard.Enter("N");
            Thread.Sleep(sleep);
            //Sprawdzenie password
            UIItemContainer window2 =  window.MdiChild(SearchCriteria.ByClassName("TGroupBox"));
            SearchCriteria searchCriteria1 = SearchCriteria.ByClassName("TPasswordEdit");
            TextBox psswd = window2.Get<TextBox>(searchCriteria1);
            Thread.Sleep(sleep);
            psswd.BulkText = "Test";
            Assert.AreEqual("Test", window2.Get<TextBox>(searchCriteria1).Name); 
            //Sprawdzenie login
            SearchCriteria searchCriteria2 = SearchCriteria.ByClassName("TEdit");
            TextBox login = window2.Get<TextBox>(searchCriteria2);
            Thread.Sleep(sleep);
            login.BulkText = "Login";
            Assert.AreEqual("Login", window2.Get<TextBox>(searchCriteria2).Name);
            //Zapisywanie do listy
            SearchCriteria searchCriteria3 = SearchCriteria.ByText("Zapisz...");
            Button save = window2.Get<Button>(searchCriteria3);
            Thread.Sleep(sleep);
            save.Click();
            UIItemContainer window3 = window.MdiChild(SearchCriteria.ByClassName("TSaveSessionDialog"));
            SearchCriteria searchCriteria4 = SearchCriteria.ByText("OK");
            Button ok = window3.Get<Button>(searchCriteria4);
            ok.Click();
            //Sprawdzenie czy się dodało
            UIItemContainer tree = window.MdiChild(SearchCriteria.ByClassName("TTreeView"));
            SearchCriteria searchCriteria5 = SearchCriteria.ByText("session");
            UIItem node = tree.Get<UIItem>(searchCriteria5);
            Assert.AreEqual("session", node.Name);
            //Usuwanie dodanej sesji
            node.Click();
            window.Keyboard.HoldKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DELETE);
            window.Keyboard.LeaveKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DELETE);
            UIItemContainer window4 = window.MdiChild(SearchCriteria.ByClassName("TMessageForm"));
            SearchCriteria searchCriteria6 = SearchCriteria.ByText("OK");
            Button confirm = window4.Get<Button>(searchCriteria4);
            confirm.Click();
            Assert.AreEqual(0, tree.Items.Count());
            //Zamkyanie okna
            window.Close();
        }
        [TestMethod]
        public void LocalDirManagerTest()
        {
            //Uruchomienie aplikacji
            App = Application.Launch(appPath);
            Thread.Sleep(sleep);
            Window window = App.GetWindow(APP_TITLE);
            //Sprawdzenie ilosci plikow w folderze lokalnym
            UIItemContainer window2 = window.MdiChild(SearchCriteria.ByClassName("TDirView"));
            Assert.AreEqual(35, window2.Items.Count);
            //Dodanie folderu 
            window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.F7);
            //Wpisanie nazwy i akceptacja
            UIItemContainer window3 = window.MdiChild(SearchCriteria.ByClassName("TCreateDirectoryDialog"));
            SearchCriteria searchCriteria = SearchCriteria.ByClassName("TEdit");
            SearchCriteria searchCriteria2 = SearchCriteria.ByText("OK");
            TextBox nameF = window3.Get<TextBox>(searchCriteria);
            Button accept = window3.Get<Button>(searchCriteria2);
            nameF.BulkText = "Test";
            accept.Click();
            //Sprawdzenie czy sie dodal
            Assert.AreEqual(41, window2.Items.Count());
            //USUWANIE
            for (int i = 0; i < 4; i++)
            {
                window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DOWN);
            }
            window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.DELETE);
            Thread.Sleep(sleep);
            //Sprawdzenie czy sie usunal
            Assert.AreEqual(35, window2.Items.Count); 
            //Przejscie do folderu
            SearchCriteria searchCriteria4 = SearchCriteria.ByText("Folder 1");
            UIItem file = window2.Get<UIItem>(searchCriteria4);
            Thread.Sleep(sleep);
            file.DoubleClick();
            //Dodawanie pliku
            window.Keyboard.HoldKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.SHIFT);
            window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.F4);
            window.Keyboard.LeaveKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.SHIFT);
            UIItemContainer window4 = window.MdiChild(SearchCriteria.ByClassName("TInputDialog"));
            SearchCriteria searchCriteria3 = SearchCriteria.ByText("OK");
            Button AC = window4.Get<Button>(searchCriteria3);
            AC.Click();
            //Sprawdzenie czy został dodany
            Assert.AreEqual(17, window2.Items.Count());
            //Cofanie z folderu
            SearchCriteria searchCriteria5 = SearchCriteria.ByText("..");
            UIItem btnBack = window2.Get<UIItem>(searchCriteria5);
            Thread.Sleep(sleep);
            btnBack.DoubleClick();
            //Zmiana nazwy
            SearchCriteria searchCriteria6 = SearchCriteria.ByText("Folder 2");
            UIItem moreInfo = window2.Get<UIItem>(searchCriteria6);
            Thread.Sleep(sleep);
            moreInfo.Click();
            window.Keyboard.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.F2);
            window.Keyboard.Enter("F");
            moreInfo.Click();
            Thread.Sleep(sleep);
            SearchCriteria searchCriteria7 = SearchCriteria.ByText("F");
            UIItem changeFile = window2.Get<UIItem>(searchCriteria7);
            Assert.AreEqual("F", changeFile.Name);
            window.Close();
        }
    }
}