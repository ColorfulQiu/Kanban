using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using kanban.main.utils;

namespace kanban.main.form
{
    public class TaskForm : Form {

        protected Label _labelUserInfo;

        protected TextBox _textBoxTask;

        protected Label _labelCategory;
        protected TextBox _textBoxCategory;

        protected Label _labelStatus;
        protected TextBox _textBoxStatus;

        protected Label _labelEstimatedTime;
        protected NumericUpDown _textBoxEstimatedTime;

        protected Label _labelInProgressTime;
        protected TextBox _textBoxInProgressTime;

        protected List<Panel> _panelCommentList;

        private static readonly int COMMENT_BOARD_WIDTH = 300;
        private static readonly int COMMENT_BOARD_HEIGHT = 100; 

        public TaskForm()
        {

            initialComponents();

            setBounds();

            addComponents();
            
            addListeners();
        }

        private void initialComponents() {
            _labelUserInfo = new Label();
            _labelUserInfo.Text = "User: Neko Gong" + "\n" + "e-mail: g405252865@163.com";

            _textBoxTask = new TextBox();
            _textBoxTask.Text = "Neko's first task";

            _labelStatus = new Label();
            _labelStatus.Text = "Status";

            _textBoxStatus = new TextBox();
            _textBoxStatus.Text = "to do";

            _labelCategory = new Label();
            _labelCategory.Text = "Category";

            _textBoxCategory = new TextBox();
            _textBoxCategory.Text = "learning";


            _labelEstimatedTime = new Label();
            _labelEstimatedTime.Text = "Estimated Time";

            _textBoxEstimatedTime = new NumericUpDown();
            _textBoxEstimatedTime.Text = "10";

            _labelInProgressTime = new Label();
            _labelInProgressTime.Text = "In Progress Time";

            _textBoxInProgressTime = new TextBox();
            _textBoxInProgressTime.Text = "-";

            _panelCommentList = new List<Panel>();
        }

        private void setBounds() {

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScroll = true;
            Icon = new Icon("board-icon.ico");
            Size = new Size(700, 600);
            Text = "Task";
            BackColor = Color.FromKnownColor(KnownColor.AliceBlue);

            _labelUserInfo.Location = new Point(70, 30);
            _labelUserInfo.Size = new Size(_labelUserInfo.PreferredWidth, _labelUserInfo.PreferredHeight);

            _textBoxTask.Location = new Point(70, 80);
            _textBoxTask.Size = new Size(450, 40);
            _textBoxTask.Font = new Font(_textBoxTask.Font.FontFamily, 15, FontStyle.Regular);
            _textBoxTask.ReadOnly = true;
            _textBoxTask.TabStop = false;

            _labelStatus.Location = new Point(70, 140);
            _labelStatus.Size = new Size(_labelStatus.PreferredWidth, _labelStatus.PreferredHeight);

            _textBoxStatus.Location = new Point(180, 140);
            _textBoxStatus.Size = new Size(80, 30);
            _textBoxStatus.ReadOnly = true;
            _textBoxStatus.TabStop = false;

            _labelCategory.Location = new Point(320, 140);
            _labelCategory.Size = new Size(_labelStatus.PreferredWidth, _labelStatus.PreferredHeight);

            _textBoxCategory.Location = new Point(430, 140);
            _textBoxCategory.Size = new Size(80, 30);
            _textBoxCategory.ReadOnly = true;
            _textBoxCategory.TabStop = false;

            _labelEstimatedTime.Location = new Point(70, 200);
            _labelEstimatedTime.Size = new Size(_labelEstimatedTime.PreferredWidth, _labelEstimatedTime.PreferredHeight);

            _textBoxEstimatedTime.Location = new Point(180, 200);
            _textBoxEstimatedTime.Size = new Size(80, 30);
            _textBoxEstimatedTime.ReadOnly = true;
            _textBoxEstimatedTime.TabStop = false;

            _labelInProgressTime.Location = new Point(320, 200);
            _labelInProgressTime.Size = new Size(_labelInProgressTime.PreferredWidth, _labelInProgressTime.PreferredHeight);

            _textBoxInProgressTime.Location = new Point(430, 200);
            _textBoxInProgressTime.Size = new Size(80, 30);
            _textBoxInProgressTime.ReadOnly = true;
            _textBoxInProgressTime.TabStop = false;
        }

        private void addComponents() {
            SuspendLayout();
            this.Controls.Add(_labelUserInfo);
            this.Controls.Add(_textBoxTask);
            this.Controls.Add(_labelStatus);
            this.Controls.Add(_textBoxStatus);
            this.Controls.Add(_labelCategory);
            this.Controls.Add(_textBoxCategory);
            this.Controls.Add(_labelEstimatedTime);
            this.Controls.Add(_textBoxEstimatedTime);
            this.Controls.Add(_labelInProgressTime);
            this.Controls.Add(_textBoxInProgressTime);
            ResumeLayout(false);
        }

        private void addListeners() {
            _textBoxTask.DoubleClick += new EventHandler(TextBoxClicked);
            _textBoxCategory.DoubleClick += new EventHandler(TextBoxClicked);
            _textBoxEstimatedTime.DoubleClick += new EventHandler(TextBoxClicked);
        }

        private void TextBoxClicked(object sender, EventArgs args) {
            if(sender is TextBox) {
                ((TextBox) sender).ReadOnly = false;
            }
            if(sender is NumericUpDown) {
                ((NumericUpDown) sender).ReadOnly = false;
            }
        }

        private void removeAllBoards() {
            for(int i = 0; i < this.Controls.Count; i++) {
                if(this.Controls[i] is Panel) {
                    Controls.Remove(Controls[i]);
                    i--;
                }
            }
        }

        private Panel generateCommonComment(string name, string comment) {
            Panel board = new Panel();
            board.Size = new Size(COMMENT_BOARD_WIDTH, COMMENT_BOARD_HEIGHT);

            Label nameLabel = new Label();
            nameLabel.Text = name + " says:";
            nameLabel.Location = new Point(10, 10);
            nameLabel.Size = new Size(nameLabel.PreferredWidth, nameLabel.PreferredHeight);

            Label commentLabel = new Label();
            commentLabel.Text = comment;
            commentLabel.Location = new Point(10, 30);
            commentLabel.Size = new Size(commentLabel.PreferredWidth, commentLabel.PreferredHeight);

            board.Controls.Add(nameLabel);
            board.Controls.Add(commentLabel);

            return board;
        }

        private void refresh() {
            removeAllBoards();
            for(int i = 0; i < _panelCommentList.Count; i++) {
                Panel commentBoard = _panelCommentList[i];
                commentBoard.Location = new Point(70, 400 + (i - 1) * (COMMENT_BOARD_HEIGHT + 10));
                this.Controls.Add(commentBoard);
            }
        }

        private void generateComment(string name, string comment) {
            Panel commentBoard = generateCommonComment(name, comment);
            commentBoard.BackColor = Utils.getLightColorFromString(name);
            _panelCommentList.Add(commentBoard);
            refresh();
        }

        public static void Main() {
            TaskForm form = new TaskForm();
            form.generateComment("neko", "What a stupid question!");
            form.generateComment("neko", "What a stupid question!");
            form.generateComment("neko", "What a stupid question!");
            form.generateComment("neko", "What a stupid question!");
            form.generateComment("neko", "What a stupid question!\nyes right.");
            form.generateComment("neko", "What a stupid question!");
            form.generateComment("neko", "What a stupid question!");
            Application.Run(form);
        }
    }
}