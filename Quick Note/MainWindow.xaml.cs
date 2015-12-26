using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Quick_Note
{
    
    public partial class MainWindow : Window
    {
        int count = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth / 1366 * 866;
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);
        }

        double beginOffset;
        double steps;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            beginOffset += steps;
            textBlock.ScrollToVerticalOffset(beginOffset);
            if (beginOffset >= vOffset)
            {
                dispatcherTimer.Stop();
            }      
        }

        TextBlocks o = null;
        double vOffset = 0;
        private void textBlocks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (o != sender as TextBlocks)
            {
                if (o != null)
                {
                    o.changerHauteurMoins();
                    o.menu_Animate(false);
                }

                o = sender as TextBlocks;

                o.changerHauteurPlus();
                beginOffset = 0;
                vOffset = itemsControl.Items.IndexOf(o) * 110;
                steps = vOffset / 30;
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
                dispatcherTimer.Start();
                o.menu_Animate(true);
            } 
        }

        private void noteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard sb = this.FindResource("animHide") as Storyboard;
            Storyboard.SetTarget(sb, noteButton);
            sb.Begin();

            noteButton.Effect =
               new DropShadowEffect
               {
                   Color = new Color { A = 10, R = 255, G = 150, B = 0 },
                   Direction = 320,
                   ShadowDepth = 0,
                   BlurRadius = 20,
                   Opacity = 100
               };
        }

        private void noteButton_MouseLeave(object sender, MouseEventArgs e)
        {

            Storyboard sb = this.FindResource("animShow") as Storyboard;
            Storyboard.SetTarget(sb, noteButton);
            sb.Begin();
        }

        private void Storyboard_Completed_noteButton(object sender, EventArgs e)
        {
            if (!noteButton.IsMouseOver)
            {
                noteButton.Effect = null;
            } 
        }

        private void textBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!redButton.IsMouseOver && 
                !redButton2.IsMouseOver && 
                !plus.IsMouseOver && 
                !textBlock.IsMouseOver && 
                !text.IsMouseOver && 
                !redButtonStack.IsMouseOver &&
                !addGrid.IsMouseOver &&
                !isOpen)
            {
                Storyboard sb = this.FindResource("animTextHide") as Storyboard;
                Storyboard.SetTarget(sb, textBlock);
                sb.Begin();
                Storyboard.SetTarget(sb, text);
                sb.Begin();

                sb = this.FindResource("animRedHide") as Storyboard;
                Storyboard.SetTarget(sb, redButton2);
                sb.Begin();
                Storyboard.SetTarget(sb, redButton);
                sb.Begin();
                sb = this.FindResource("animRedStackHide") as Storyboard;
                Storyboard.SetTarget(sb, redButtonStack);
                sb.Begin();
            }
   
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {

            if (!redButton.IsMouseOver &&
                !redButton2.IsMouseOver &&
                !plus.IsMouseOver && 
                !textBlock.IsMouseOver && 
                !text.IsMouseOver &&
                !redButtonStack.IsMouseOver &&
                !addGrid.IsMouseOver &&
                !isOpen)
            {
                textBlock.Visibility = Visibility.Hidden;
                noteButton.Visibility = Visibility.Visible;
            }
        }

        private void redButton_MouseEnter(object sender, MouseEventArgs e)
        {
            redButton.Effect =
                                new DropShadowEffect
                                {
                                    Color = new Color { A = 100, R = 255, G = 0, B = 0 },
                                    Direction = 320,
                                    ShadowDepth = 0,
                                    BlurRadius = 20,
                                    Opacity = 10
                                };

            if (redButtonStack.Height < 302)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation
                {

                    To = 302,
                    Duration = TimeSpan.FromSeconds(0.3)
                };


                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(StackPanel.HeightProperty));
                Storyboard.SetTarget(widthAnimation, redButtonStack);
                Storyboard s = new Storyboard();
                s.Children.Add(widthAnimation);
                s.Begin();
                Storyboard sb = this.FindResource("animStackShow") as Storyboard;
                Storyboard.SetTarget(sb, redButtonStack);
                sb.Begin();                

                var da = new DoubleAnimation(0, 180, new Duration(TimeSpan.FromSeconds(0.3)));
                da.Completed += redButtonStoryboard_Completed;
                var rt = new RotateTransform();
                plus.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
               
            }
           
        }

        private void redButton_MouseLeave(object sender, MouseEventArgs e)
        {
            redButton.Effect =
                                new DropShadowEffect
                                {
                                    Color = new Color { A = 100, R = 255, G = 0, B = 0 },
                                    Direction = 320,
                                    ShadowDepth = 0,
                                    BlurRadius = 10,
                                    Opacity = 10
                                };

            if (!redButtonStack.IsMouseOver && !redButton2.IsMouseOver)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(StackPanel.HeightProperty));
                Storyboard.SetTarget(widthAnimation, redButtonStack);
                Storyboard s = new Storyboard();
                s.Children.Add(widthAnimation);
                s.Begin();

                Storyboard sb = this.FindResource("animStackHide") as Storyboard;
                Storyboard.SetTarget(sb, redButtonStack);
                sb.Begin();

                plusButton.Opacity = 1;

                var da = new DoubleAnimation(180, 0, new Duration(TimeSpan.FromSeconds(0.3)));
                var rt = new RotateTransform();
                plus.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);

            }
        }

        private void redButtonStoryboard_Completed(object sender, EventArgs e)
        {
            if (redButton2.IsMouseOver)
            {
                plusButton.Opacity = 0.01;  
            }
                
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.Effect =
                                new DropShadowEffect
                                {
                                    Color = new Color { A = 100, R = 0, G = 0, B = 0 },
                                    Direction = 320,
                                    ShadowDepth = 0,
                                    BlurRadius = 10,
                                    Opacity = 10
                                };
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.Effect = null;
        }

        bool isOpen = false;
        bool deleteAll = false;
        private void noteDelte_Click(object sender, RoutedEventArgs e)
        {
            deleteQuestion.Content = "Are you sure you want to delete this Quick Note ?";
            isOpen = true;
            deleteDialog.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, deleteDialog);
            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();
            o = sender as TextBlocks;
            deleteAll = false;
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, deleteDialog);
            Storyboard s = new Storyboard();
            s.Completed += sDelete_Completed;
            s.Children.Add(widthAnimation);
            s.Begin();
        }

        private void sDelete_Completed(object sender, EventArgs e)
        {
            deleteDialog.Visibility = Visibility.Hidden;
            isOpen = false;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                if (!deleteAll)
                {
                    string save;
                    string saveOld;
                    save = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + o.nomBlock + "[" + o.dateBlock.Replace("Date . ", "") + "].rtf";

                    saveOld = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + o.nomBlock + "[" + o.dateBlock.Replace("Date . ", "") + "].rtf";


                    if (File.Exists(save))
                    {

                        File.Delete(save);
                    }
                    else if (File.Exists(saveOld))
                    {
                        File.Delete(saveOld);
                    }
                    itemsControl.Items.Remove(o);
                    count--;
                    if (!itemsControl.HasItems)
                    {
                        text.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    while (itemsControl.HasItems)
                    {
                        string save;
                        string saveOld;

                        save = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + ((TextBlocks)itemsControl.Items.GetItemAt(0)).nomBlock + "[" + ((TextBlocks)itemsControl.Items.GetItemAt(0)).dateBlock.Replace("Date . ", "") + "].rtf";

                        saveOld = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + ((TextBlocks)itemsControl.Items.GetItemAt(0)).nomBlock + "[" + ((TextBlocks)itemsControl.Items.GetItemAt(0)).dateBlock.Replace("Date . ", "") + "].rtf";


                        if (File.Exists(save))
                        {

                            File.Delete(save);
                        }
                        else if (File.Exists(saveOld))
                        {
                            File.Delete(saveOld);
                        }
                        itemsControl.Items.RemoveAt(0);
                    }
                    count = 0;
                    text.Visibility = Visibility.Visible;
                    
                }
            }
            catch 
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
            finally
            {
                isOpen = false;
                noButton_Click(noButton, new RoutedEventArgs());
            }
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Visibility = Visibility.Visible;
            Storyboard sb = this.FindResource("animTextShow") as Storyboard;
            Storyboard.SetTarget(sb, textBlock);
            sb.Begin();
            Storyboard.SetTarget(sb, text);
            sb.Begin();
            sb = this.FindResource("animRedShow") as Storyboard;
            Storyboard.SetTarget(sb, redButton2);
            sb.Begin();
            Storyboard.SetTarget(sb, redButton);
            sb.Begin();
            sb = this.FindResource("animRedStackShow") as Storyboard;
            Storyboard.SetTarget(sb, redButtonStack);
            sb.Begin();
            noteButton.Visibility = Visibility.Hidden;
            textBlock.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 10, R = 255, G = 150, B = 0 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 20,
                        Opacity = 10
                    };
        }

        private void redButton_Click(object sender, RoutedEventArgs e)
        {

            addGrid.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.8)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, addGrid);
            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();
            
        }

        bool added = false;
        private void add_Click(object sender, RoutedEventArgs e)
        {
            TextBlocks textBlocks;
            if (addName.Text == "")
            {
                textBlocks = new TextBlocks("Quick Note n°"+Convert.ToString(count+1),(bool)addChecked.IsChecked,addDate.Text,false);
            }
            else
            {
                textBlocks = new TextBlocks(Convert.ToString(addName.Text), (bool)addChecked.IsChecked, addDate.Text,false);
            }   
            textBlocks.Click += noteDelte_Click;
            textBlocks.GotFocus += textBlocks_GotFocus;
            itemsControl.Items.Add(textBlocks);
            count++;
            text.Visibility = Visibility.Hidden;
            
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, addGrid);
            Storyboard s = new Storyboard();
            s.Completed += s_Completed;
            s.Children.Add(widthAnimation);
            s.Begin();

            // Expand the new textBlocks and Collapse the previous expanded
            if (o != null)
            {
                o.changerHauteurMoins();
                o.menu_Animate(false);
            }
            
            o = textBlocks;
            added = true;
            addChecked.IsChecked = false;
            addName.Text = "";
            addDate.Text = "";
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {      
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, addGrid);
            Storyboard s = new Storyboard();
            s.Completed += s_Completed; 
            s.Children.Add(widthAnimation);
            s.Begin();

            addChecked.IsChecked = false;
            added = false;
            addName.Text = "";
            addDate.Text = "";
        }

        private void s_Completed(object sender, EventArgs e)
        {
            addGrid.Visibility = Visibility.Hidden;
            if (added)
            {
                o.changerHauteurPlus();
                textBlock.ScrollToBottom();
                o.menu_Animate(true);
            }
        }

        private void addChecked_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)addChecked.IsChecked)
            {
                addDate.Text = "" + DateTime.Now;
                addDate.IsEnabled = false;
            }
            else
            {
                addDate.IsEnabled = true;
                addDate.Text = "";
            }
            
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    ((TextBlocks)itemsControl.Items.GetItemAt(i)).saveDocument();
                } 
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    ((TextBlocks)itemsControl.Items.GetItemAt(i)).saveDocument();
                } 
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
            
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    ((TextBlocks)itemsControl.Items.GetItemAt(i)).exportDocument();
                }
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteQuestion.Content = "Are you sure you want to delete All the Quick Notes ?";
            deleteDialog.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.OpacityProperty));
            Storyboard.SetTarget(widthAnimation, deleteDialog);
            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();
            isOpen = true;
            deleteAll = true;
        }

        private void facebook_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/mohammed.handaoui");
        }

        private void email_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://plus.google.com/+mohamedHANDAOUI");
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                add_Click(add,new RoutedEventArgs ());
            }
            else if (e.Key == Key.Escape)
            {
                 cancel_Click(add,new RoutedEventArgs ());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TextBlocks textBlocks = new TextBlocks("Quick Note n°" + Convert.ToString(count + 1), (bool)addChecked.IsChecked, addDate.Text,false);
                textBlocks.Click += noteDelte_Click;
                textBlocks.GotFocus += textBlocks_GotFocus;
                itemsControl.Items.Add(textBlocks);
                count++;
                text.Visibility = Visibility.Hidden;
                if (o != null)
                {
                    o.changerHauteurMoins();
                    o.menu_Animate(false);
                }
                o = textBlocks;
                o.changerHauteurPlus();
                textBlock.ScrollToBottom();
                o.menu_Animate(true);
            }
        }

        bool started = true;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (started)
            {
                Storyboard start = this.FindResource("animHide") as Storyboard;
                start.Completed += start_Completed;
                Storyboard.SetTarget(start, noteButton);
                start.Begin();

                noteButton.Effect =
                   new DropShadowEffect
                   {
                       Color = new Color { A = 10, R = 255, G = 150, B = 0 },
                       Direction = 320,
                       ShadowDepth = 0,
                       BlurRadius = 20,
                       Opacity = 100
                   };
                
            }

            if (Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes"))
            {
                DirectoryInfo taskDirectory = new DirectoryInfo(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes");
                FileInfo[] taskFiles = taskDirectory.GetFiles("*.rtf");

                DateTime[] creationTimes = new DateTime[taskFiles.Length];
                for (int i = 0; i < taskFiles.Length; i++)
                    creationTimes[i] = taskFiles[i].CreationTime;

                Array.Sort(creationTimes, taskFiles);

                if (taskFiles.Length != 0)
                {
                    for (int i = 0; i < taskFiles.Length; i++)
                    {
                        if (taskFiles[i].Extension == ".rtf")
                        {
                            string[] name_date = taskFiles[i].Name.Replace(".rtf", "").Replace("]", "").Split('[');
                            bool fav = false;
                            if (taskFiles[i].Name.Contains("❤"))
                            {
                                name_date[0] = name_date[0].Replace("❤", "");
                                fav = true;
                            }

                            TextBlocks textBlock = new TextBlocks(name_date[0], false, name_date[1], fav);
                            textBlock.Click += noteDelte_Click;
                            textBlock.GotFocus += textBlocks_GotFocus;
                            textBlock.loadDocument(taskFiles[i].FullName);
                            text.Visibility = Visibility.Hidden;
                            itemsControl.Items.Add(textBlock);
                        }
                    }
                }
            }

            
            
        }

        void start_Completed(object sender, EventArgs e)
        {
            if (started)
            {
                Storyboard sb = this.FindResource("animShowStart") as Storyboard;
                Storyboard.SetTarget(sb, noteButton);
                sb.Begin();
                started = false;
            }
            
        }       
    }
}
