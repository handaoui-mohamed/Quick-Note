using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Quick_Note
{

    public partial class TextBlocks : UserControl
    {
   

        public TextBlocks(String name, bool isChecked,String date,bool fav)
        {
            InitializeComponent();
            if(date == "")
            {
                blockDate.Content = ("Date : " + DateTime.Now).Replace('-','/').Replace('.',':');
            }
            else
            {
                blockDate.Content = ("Date : " + date).Replace('-', '/').Replace('.', ':');
            }
            
            blockName.Content = name;
            if (fav)
            {
                star.Fill = System.Windows.Media.Brushes.Orange;
                star.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 20,
                    Opacity = 100
                };
                clicked = true;
                star.Opacity = 1;
            }
        }

        public bool fav
        {
            get { return clicked; }
            set { clicked = value; }
        }

        public string dateBlock
        {
            get { return ((string)blockDate.Content).Replace('/', '-').Replace(':', '.'); }
            set { blockDate.Content = value; }
        }

        public string nomBlock
        {
            get { return (string)blockName.Content; }
            set { blockName.Content = value; }
        }

        public void changerHauteurPlus()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                To = 360,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Window.HeightProperty));
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();
            blockTexing.Cursor = Cursors.Pen;
        }

        public void changerHauteurMoins()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                To = 110.488,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Window.HeightProperty));
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();
            blockTexing.Cursor = Cursors.Hand;
        }

        private void Viewbox_MouseEnter(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.Opacity = 1;
        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.Opacity = 0.5;
        }
       
        public event RoutedEventHandler Click;

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        bool clicked = false;
        private void star_Click(object sender, RoutedEventArgs e)
        {

            if (star.Fill == System.Windows.Media.Brushes.Orange)
            {
                star.Fill = System.Windows.Media.Brushes.Gray;
                star.Effect = null;
                clicked = false;
            }
            else
            {
                star.Fill = System.Windows.Media.Brushes.Orange;
                star.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 20,
                    Opacity = 100
                };
                clicked = true;
                star.Opacity = 1;
            }
        }

        bool open = true;
        private void menu_Click(object sender, RoutedEventArgs e)
        {
            
            if (open)
            {
                open = false;
            }
            else
            {
                open = true;
            }
            menu_Animate(open);
        }

        public void menu_Animate (bool open)
        {
            if (open)
            {
                if (toolStack.Width == 0)
                {
                    DoubleAnimation widthAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 410,
                        Duration = TimeSpan.FromSeconds(0.3)
                    };
                    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(StackPanel.WidthProperty));
                    Storyboard.SetTarget(widthAnimation, toolStack);
                    Storyboard s = new Storyboard();
                    s.Children.Add(widthAnimation);
                    s.Begin();
                }
                
            }
            else
            {
                if (toolStack.Width == 410)
                {
                    DoubleAnimation widthAnimation = new DoubleAnimation
                    {
                        From = 410,
                        To = 0,
                        Duration = TimeSpan.FromSeconds(0.3)
                    };
                    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(StackPanel.WidthProperty));
                    Storyboard.SetTarget(widthAnimation, toolStack);
                    Storyboard s = new Storyboard();
                    s.Children.Add(widthAnimation);
                    s.Begin();
                }
                
            }
        }
       
        private void star_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!clicked)
            {
                star.Opacity = 1;
            }
            
        }

        private void star_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!clicked)
            {
                star.Opacity = 0.5;
            }
            
        }

        //Change to Upper or Lower case
        bool upper = false;
        private void upperButton_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selection= blockTexing.Selection;
            if (!upper)
            {
                selection.Text = selection.Text.ToUpper();
                upper = true;
            }
            else
            {
                selection.Text = selection.Text.ToLower();
                upper = false;
            }
            
        }

        //Change to Itallic or Normal style
        bool itallic = false;
        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selection = blockTexing.Selection;
            if (!itallic)
            {
                selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                itallic = true;
            }
            else
            {
                selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
                itallic = false;
            }
            
        }

        //Change to Bold or Normal weight
        bool bold = false;
        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selection = blockTexing.Selection;
            if (!bold)
            {
                selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                bold = true;
            }
            else
            {
                selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                bold = false;
            }
        }

        //Change Alignement to left
        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;
            selectionRange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }

        private void centerButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;
            selectionRange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;
            selectionRange.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        //Change Font size
        double size = 20;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextSelection selection = blockTexing.Selection;
            
            if (!selection.IsEmpty)
            {
                size = Convert.ToDouble(((TextBlock)fontCombo.SelectedValue).Text);
                selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
                
            }
            else
            {
                blockTexing.FontSize = Convert.ToDouble(((TextBlock)fontCombo.SelectedValue).Text);
            }

        }

        private void minusSizeButton_Click(object sender, RoutedEventArgs e)
        {
           
            TextSelection selection = blockTexing.Selection;
            
            if (!selection.IsEmpty && size >= 6)
            {
                
                selection.ApplyPropertyValue(TextElement.FontSizeProperty, size--);
            }
        }

        private void plusSizeButton_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selection = blockTexing.Selection;

            if (!selection.IsEmpty)
            {

                selection.ApplyPropertyValue(TextElement.FontSizeProperty, size++);
            }
        }

        // cross the selected text
        bool crossed = false;
        private void crossButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;

            if (!crossed)
            {
                selectionRange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                crossed = true;
            }
            else
            {
                selectionRange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                crossed = false;
            }

        }

        //Underline the selected text
        bool undelined = false;
        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;
            if (!undelined)
            {
                selectionRange.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                undelined = true;
            }
            else
            {
                selectionRange.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                undelined = false;
            }

        }

        //HighLight selected text
        bool highLighted = false;
        private void highLightButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange selectionRange = blockTexing.Selection as TextRange;
            if (!highLighted)
            {
                selectionRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Color.FromRgb(255, 255, 0)));
                highLighted = true;
            }
            else
            {
                selectionRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, null);
                highLighted = false;
            }
        }        

        // Focused on TextBlock
        public event RoutedEventHandler GotFocus;
        private void Focused_RichTextBlock(object sender, RoutedEventArgs e)
        {
            if (GotFocus != null)
            {
                GotFocus(this, e);
            }
        }

        // Saving/Loading and exporting the RichtextBox Content in RTF file

        private void btnSave_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                if (!Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes");
                }

                TextRange tr = new TextRange(blockTexing.Document.ContentStart, blockTexing.Document.ContentEnd);
                MemoryStream ms = new MemoryStream();
                tr.Save(ms, DataFormats.Rtf);


                FileStream file;
                string old;
                if (clicked)
                {
                    old = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                    if (File.Exists(old))
                    {
                        File.Delete(old);
                    }
                    file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

                }
                else
                {
                    old = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                    if (File.Exists(old))
                    {
                        File.Delete(old);
                    }
                    file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

                }

                ms.WriteTo(file);
                file.Close();
                ms.Close(); 
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
            
        } 

        public void saveDocument()
        {

            if (!Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes");
            }

            TextRange tr = new TextRange(blockTexing.Document.ContentStart, blockTexing.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);


            FileStream file;
            string old;
            if (clicked)
            {
                old = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                if (File.Exists(old))
                {
                    File.Delete(old);
                }
                file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

            }
            else
            {
                old = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                if (File.Exists(old))
                {
                    File.Delete(old);
                }
                file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

            }

            ms.WriteTo(file);
            file.Close();
            ms.Close();
        }

        public void loadDocument(string save)
        {
            try
            {
                // Read the file
                if (File.Exists(save))
                {
                    TextRange tr = new TextRange(blockTexing.Document.ContentStart, blockTexing.Document.ContentEnd);
                    MemoryStream ms = new MemoryStream();
                    using (FileStream fs = File.OpenRead(save))
                    {
                        fs.CopyTo(ms);
                    }
                    tr.Load(ms, DataFormats.Rtf);
                }
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
            
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes");
                }

                TextRange tr = new TextRange(blockTexing.Document.ContentStart, blockTexing.Document.ContentEnd);
                MemoryStream ms = new MemoryStream();
                tr.Save(ms, DataFormats.Rtf);


                FileStream file;
                string old;
                if (clicked)
                {
                    old = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                    if (File.Exists(old))
                    {
                        File.Delete(old);
                    }
                    file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

                }
                else
                {
                    old = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                    if (File.Exists(old))
                    {
                        File.Delete(old);
                    }
                    file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

                }

                ms.WriteTo(file);
                file.Close();
                ms.Close();
            }
            catch
            {
                MessageBox.Show("Please verify that the file is note open", "Error!");
            }
        }

        public void exportDocument()
        {

            if (!Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes");
            }

            TextRange tr = new TextRange(blockTexing.Document.ContentStart, blockTexing.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);


            FileStream file;
            string old;
            if (clicked)
            {
                old = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                if (File.Exists(old))
                {
                    File.Delete(old);
                }
                file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

            }
            else
            {
                old = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\❤" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf";
                if (File.Exists(old))
                {
                    File.Delete(old);
                }
                file = new FileStream(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\QuickNotes\" + nomBlock + "[" + dateBlock.Replace("Date . ", "") + "].rtf", FileMode.Create, FileAccess.Write);

            }

            ms.WriteTo(file);
            file.Close();
            ms.Close();
        }
    }
}
