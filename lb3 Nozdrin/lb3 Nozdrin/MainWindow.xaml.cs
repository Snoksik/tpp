using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lb3_Nozdrin
{
    public partial class MainWindow : Window
    {
        private const string NotesFileName = "notes.txt";

        private List<string> allNotes = new List<string>();
        private List<string> filteredNotes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
            UpdateNotesListBox();
            InitializeContextMenu();
        }

        private void LoadNotes()
        {
            if (File.Exists(NotesFileName))
            {
                allNotes = File.ReadAllLines(NotesFileName).ToList();
            }
        }

        private void SaveNotes()
        {
            File.WriteAllLines(NotesFileName, allNotes);
        }

        private void UpdateNotesListBox()
        {
            string searchText = SearchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredNotes = new List<string>(allNotes);
            }
            else
            {
                filteredNotes = allNotes.Where(n => n.ToLower().Contains(searchText)).ToList();
            }

            NotesListBox.ItemsSource = null;
            NotesListBox.ItemsSource = filteredNotes;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newNote = NoteTextBox.Text.Trim();
            if (string.IsNullOrEmpty(newNote))
            {
                MessageBox.Show("Текст заметки не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            allNotes.Add(newNote);
            NoteTextBox.Clear();
            UpdateNotesListBox();
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotesListBox.SelectedIndex == -1)
            {
                NoteTextBox.Clear();
                return;
            }
            string selectedNote = (string)NotesListBox.SelectedItem;
            NoteTextBox.Text = selectedNote;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку для сохранения изменений.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string updatedNote = NoteTextBox.Text.Trim();
            if (string.IsNullOrEmpty(updatedNote))
            {
                MessageBox.Show("Текст заметки не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int indexInAll = allNotes.IndexOf(filteredNotes[selectedIndex]);
            if (indexInAll != -1)
            {
                allNotes[indexInAll] = updatedNote;
                UpdateNotesListBox();
                NotesListBox.SelectedIndex = selectedIndex;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить выбранную заметку?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                allNotes.Remove(filteredNotes[selectedIndex]);
                NoteTextBox.Clear();
                UpdateNotesListBox();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveNotes();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateNotesListBox();
        }

        // Новый метод инициализации контекстного меню
        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenu();

            var colors = new Dictionary<string, Color>()
            {
                {"Белый", Colors.White},
                {"Жёлтый", Colors.LightYellow},
                {"Зелёный", Colors.LightGreen},
                {"Голубой", Colors.LightSkyBlue},
                {"Розовый", Colors.LightPink},
                {"Серый", Colors.LightGray}
            };

            var changeTextBoxBackground = new MenuItem { Header = "Изменить цвет фона поля ввода" };
            foreach (var kvp in colors)
            {
                var item = new MenuItem { Header = kvp.Key };
                item.Click += (s, args) =>
                {
                    NoteTextBox.Background = new SolidColorBrush(kvp.Value);
                };
                changeTextBoxBackground.Items.Add(item);
            }

            var changeWindowBackground = new MenuItem { Header = "Изменить цвет фона окна" };
            foreach (var kvp in colors)
            {
                var item = new MenuItem { Header = kvp.Key };
                item.Click += (s, args) =>
                {
                    this.Background = new SolidColorBrush(kvp.Value);
                };
                changeWindowBackground.Items.Add(item);
            }

            contextMenu.Items.Add(changeTextBoxBackground);
            contextMenu.Items.Add(changeWindowBackground);

            NoteTextBox.ContextMenu = contextMenu;
        }
    }
}
