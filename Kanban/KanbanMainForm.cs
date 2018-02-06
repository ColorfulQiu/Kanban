using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace kanban.main.form
{
    public class PanelTag {
        public int taskId { set; get; }
        public int column { set; get; }     // 0: to do, 1: in progress, 2: done
        public int row {set; get; }
    }

    public class KanbanMainForm : Form {
		protected Label _labelUserInfo;
        protected CheckBox _checkBoxShowMineOnly;
        protected Button _buttonShowOverdueTask;
        protected Button _buttonCreateNewTask;

        protected List<Panel> _panelToDoList;
        protected List<Panel> _panelInProgessList;
        protected List<Panel> _panelDoneList;

        private static readonly int FORM_WIDTH = 1000;
        private static readonly int WIDTH_INTERVAL = 10;
        private static readonly int LEFT_MARGIN = WIDTH_INTERVAL;
        private static readonly int RIGHT_MARGIN = 40;
        private static readonly int BOARD_WIDTH = (FORM_WIDTH - WIDTH_INTERVAL * 2 - LEFT_MARGIN - RIGHT_MARGIN) / 3;
        private static readonly int BOARD_HEIGHT = 80;
        private static readonly int HEIGHT_INTERVAL = 10;
        private static readonly Point TODO_FIRST_POSITION = new Point(LEFT_MARGIN, 200);
        private static readonly Point INPROGRESS_FIRST_POSITION = new Point(BOARD_WIDTH + LEFT_MARGIN + WIDTH_INTERVAL, 200);
        private static readonly Point DONE_FIRST_POSITION = new Point(BOARD_WIDTH * 2 + LEFT_MARGIN + WIDTH_INTERVAL * 2, 200);

        private bool boardClicked = false;
        private Point boardOriginPosition = new Point();
        private Point mouseOriginPosition = new Point();

        public KanbanMainForm()
        {

            initialComponents();

            setBounds();

            addComponents();
            
            addListeners();
        }

        private void initialComponents() {
            _labelUserInfo = new Label();
            _labelUserInfo.Text = "User: Neko Gong" + "\n" + "e-mail: g405252865@163.com";

            _checkBoxShowMineOnly = new CheckBox();
            _checkBoxShowMineOnly.Text = "Show my bord only";

            _buttonShowOverdueTask = new Button();
            _buttonShowOverdueTask.Text = "Show overdue tasks";

            _buttonCreateNewTask = new Button();
            _buttonCreateNewTask.Text = "Create new task";

            _panelToDoList = new List<Panel>();
            _panelInProgessList = new List<Panel>();
            _panelDoneList = new List<Panel>();
        }

        private void setBounds() {

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScroll = true;
            Icon = new Icon("board-icon.ico");
            Size = new Size(1000, 800);

            _labelUserInfo.Location = new Point(70, 30);
            _labelUserInfo.Size = new Size(_labelUserInfo.PreferredWidth, _labelUserInfo.PreferredHeight);

            _checkBoxShowMineOnly.Location = new Point(400, 30);
            _checkBoxShowMineOnly.Size = new Size(150, 30);

            _buttonCreateNewTask.Location = new Point(650, 30);
            _buttonCreateNewTask.Size = new Size(120, 30);

            _buttonShowOverdueTask.Location = new Point(780, 30);
            _buttonShowOverdueTask.Size = new Size(120, 30);
        }

        private void addComponents() {
            SuspendLayout();
            this.Controls.Add(_labelUserInfo);
            this.Controls.Add(_checkBoxShowMineOnly);
            this.Controls.Add(_buttonShowOverdueTask);
            this.Controls.Add(_buttonCreateNewTask);
            ResumeLayout(false);
        }

        private void addListeners() {
            _buttonShowOverdueTask.Click += new EventHandler(showOverdueTask);
        }

        private void removeAllBoards() {
            for(int i = 0; i < this.Controls.Count; i++) {
                if(this.Controls[i] is Panel) {
                    Controls.Remove(Controls[i]);
                    i--;
                }
            }
        }

        private Panel generateCommonBoard(string task, char name) {
            Panel board = new Panel();
            board.Size = new Size(BOARD_WIDTH, BOARD_HEIGHT);

            int margin = 5;
            int sizeOfName = BOARD_HEIGHT - 2 * margin;
            
            Label taskLabel = new Label();
            taskLabel.Text = task;
            taskLabel.Location = new Point(margin, margin);
            taskLabel.Size = new Size(BOARD_WIDTH - 3 * margin - sizeOfName, sizeOfName);
            taskLabel.BackColor = Color.Yellow;
            taskLabel.TextAlign = ContentAlignment.MiddleLeft;
            taskLabel.Font = new Font(taskLabel.Font.Name, taskLabel.Font.Size + 2, FontStyle.Regular);

            Label nameLabel = new Label();
            nameLabel.Text = "" + name;
            nameLabel.Location = new Point(BOARD_WIDTH - margin - sizeOfName, margin);
            nameLabel.Size = new Size(sizeOfName, sizeOfName);
            nameLabel.BackColor = Color.Yellow;
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            nameLabel.Font = new Font(nameLabel.Font.Name, nameLabel.Font.Size + 10, FontStyle.Bold);

            board.Controls.Add(taskLabel);
            board.Controls.Add(nameLabel);

            board.MouseDown += new MouseEventHandler(boardMouseDown);
            board.MouseMove += new MouseEventHandler(boardMouseMove);
            board.MouseUp += new MouseEventHandler(boardMouseUp);
            taskLabel.MouseDown += new MouseEventHandler(boardMouseDown);
            taskLabel.MouseMove += new MouseEventHandler(boardMouseMove);
            taskLabel.MouseUp += new MouseEventHandler(boardMouseUp);
            nameLabel.MouseDown += new MouseEventHandler(boardMouseDown);
            nameLabel.MouseMove += new MouseEventHandler(boardMouseMove);
            nameLabel.MouseUp += new MouseEventHandler(boardMouseUp);

            return board;
        }

        private void refresh() {
            removeAllBoards();
            for(int i = 0; i < _panelToDoList.Count; i++) {
                Panel panelBoard = _panelToDoList[i];
                panelBoard.Location = new Point(TODO_FIRST_POSITION.X, TODO_FIRST_POSITION.Y + (i - 1) * (BOARD_HEIGHT + HEIGHT_INTERVAL));
                this.Controls.Add(panelBoard);
            }
            for(int i = 0; i < _panelInProgessList.Count; i++) {
                Panel panelBoard = _panelInProgessList[i];
                panelBoard.Location = new Point(INPROGRESS_FIRST_POSITION.X, INPROGRESS_FIRST_POSITION.Y + (i - 1) * (BOARD_HEIGHT + HEIGHT_INTERVAL));
                this.Controls.Add(panelBoard);
            }
            for(int i = 0; i < _panelDoneList.Count; i++) {
                Panel panelBoard = _panelDoneList[i];
                panelBoard.Location = new Point(DONE_FIRST_POSITION.X, DONE_FIRST_POSITION.Y + (i - 1) * (BOARD_HEIGHT + HEIGHT_INTERVAL));
                this.Controls.Add(panelBoard);
            }
        }

        public void generateToDoBoard(string task, char name) {
            Panel panelBoard = generateCommonBoard(task, name);
            panelBoard.Tag = new PanelTag () { column = 0, row = _panelToDoList.Count };
            panelBoard.BackColor = Color.Green;
            _panelToDoList.Add(panelBoard);
            refresh();
        }

        public void generateInProgressBoard(string task, char name) {
            Panel panelBoard = generateCommonBoard(task, name);
            panelBoard.Tag = new PanelTag () { column = 1, row = _panelInProgessList.Count };
            panelBoard.BackColor = Color.Blue;
            _panelInProgessList.Add(panelBoard);
            refresh();
        }

        public void generateDoneBoard(string task, char name) {
            Panel panelBoard = generateCommonBoard(task, name);
            panelBoard.Tag = new PanelTag () { column = 2, row = _panelDoneList.Count };
            panelBoard.BackColor = Color.Red;
            _panelDoneList.Add(panelBoard);
            refresh();
        }

        // Listeners

        private void showOverdueTask(object sender, EventArgs args) {
            this.generateToDoBoard("1This is my first task, and I should finish the C# kanban board", 'N');
        }

        private void boardMouseDown(object sender, MouseEventArgs args) {
            Panel board = sender is Panel? (Panel) sender: (Panel) (((Control) sender).Parent);
            board.BringToFront();
            boardClicked = true;
            boardOriginPosition = board.Location;
            mouseOriginPosition = new Point(args.X + board.Location.X, args.Y + board.Location.Y);
        }

        private void boardMouseMove(object sender, MouseEventArgs args) {
            Panel board = sender is Panel? (Panel) sender: (Panel) (((Control) sender).Parent);
            if(boardClicked) {
                Point mousePostion = new Point(args.X + board.Location.X, args.Y + board.Location.Y);
                board.Location = new Point(mousePostion.X - mouseOriginPosition.X + boardOriginPosition.X, 
                                           mousePostion.Y - mouseOriginPosition.Y + boardOriginPosition.Y);
            }
        }

        private void boardMouseUp(object sender, MouseEventArgs args) {
            Panel board = sender is Panel? (Panel) sender: (Panel) (((Control) sender).Parent);
            boardClicked = false;
            boardOriginPosition = new Point();
            mouseOriginPosition = new Point();

            Point boardCenter = new Point(board.Location.X + BOARD_WIDTH / 2, board.Location.Y + BOARD_HEIGHT / 2);
            Console.WriteLine("center: " + boardCenter);
            int targetStatus = -1;
            if(boardCenter.X < INPROGRESS_FIRST_POSITION.X) {
                targetStatus = 0;
            } else if(boardCenter.X < DONE_FIRST_POSITION.X) {
                targetStatus = 1;
            } else {
                targetStatus = 2;
            }

            movingBoardToStatus(board, targetStatus);
        }

        private void movingBoardToStatus(Panel board, int targetStatus) {
            PanelTag tag = (PanelTag) (board.Tag);
            Console.WriteLine(tag.column + ", " + tag.row);
            Console.WriteLine("targetStatus = " + targetStatus);
            if(tag.column != targetStatus) {
                if(tag.column == 0) {
                    _panelToDoList.RemoveAt(tag.row);
                } else if(tag.column == 1) {
                    _panelInProgessList.RemoveAt(tag.row);
                } else if(tag.column == 2) {
                    _panelDoneList.RemoveAt(tag.row);
                }

                if(targetStatus == 0) {
                    board.Tag = new PanelTag() { column = targetStatus, row = _panelToDoList.Count };
                    _panelToDoList.Add(board);
                } else if(targetStatus == 1) {
                    board.Tag = new PanelTag() { column = targetStatus, row = _panelInProgessList.Count };
                    _panelInProgessList.Add(board);
                } else if(targetStatus == 2) {
                    board.Tag = new PanelTag() { column = targetStatus, row = _panelDoneList.Count };
                    _panelDoneList.Add(board);
                }
            }

            for(int i = 0; i < _panelToDoList.Count; i++) {
                _panelToDoList[i].Tag = new PanelTag() { column = 0, row = i };
            }
            for(int i = 0; i < _panelInProgessList.Count; i++) {
                _panelInProgessList[i].Tag = new PanelTag() { column = 1, row = i };
            }
            for(int i = 0; i < _panelDoneList.Count; i++) {
                _panelDoneList[i].Tag = new PanelTag() { column = 2, row = i };
            }

            refresh();
        }

        public static void Main() {
            KanbanMainForm form = new KanbanMainForm();
            form.generateToDoBoard("1This is my first task, and I should finish the C# kanban board", 'N');
            form.generateToDoBoard("2This is my first task, and I should finish the C# kanban board", 'N');

            form.generateInProgressBoard("3This is my first task, and I should finish the C# kanban board", 'N');
            form.generateInProgressBoard("4This is my first task, and I should finish the C# kanban board", 'N');

            form.generateDoneBoard("5This is my first task, and I should finish the C# kanban board", 'N');
            form.generateDoneBoard("6This is my first task, and I should finish the C# kanban board", 'N');
            form.generateDoneBoard("7This is my first task, and I should finish the C# kanban board hhahahahahahahahahahahahahahahahahahahahahahaha ahahahah aaaaaaaaaaaaaaaaaaaaaaaaa", 'N');
            Application.Run(form);
        }
    }
}