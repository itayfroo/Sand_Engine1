using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

public class DrawingForm : Form
{
    //properties
    int red = 130;
    int green = 12;
    int blue = 234;
    int x = 0;
    int y = 0;
    bool mouseClicked = false;
    int[,] arr = new int[800, 600];

    // Use System.Windows.Forms.Timer explicitly
    System.Windows.Forms.Timer timer;

    public DrawingForm()
    {
        this.Text = "Fixed Size Form";
        this.Size = new Size(800, 600);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        this.Paint += DrawingForm_Paint;
        this.BackgroundImage = Image.FromFile("C:\\c#\\sand\\sand\\Arena.jpg");  
        this.BackgroundImageLayout = ImageLayout.Stretch;
        // Mouse events
        this.MouseDown += DrawingForm_MouseDown;
        this.MouseUp += DrawingForm_MouseUp;
        this.MouseMove += DrawingForm_MouseMove;

        // Initialize and start the timer
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 50;  
        timer.Tick += Timer_Tick;
    }
    // Capture mouse coordinates
    private void DrawingForm_MouseDown(object sender, MouseEventArgs e)
    {
        mouseCords(e);  // Capture mouse coordinates
        mouseClicked = true;  
        timer.Start();  
    }
    // Stop the timer when the mouse button is released
    private void DrawingForm_MouseUp(object sender, MouseEventArgs e)
    {
        mouseClicked = false; 
        timer.Stop();  
    }
    // Update mouse coordinates while moving the mouse
    private void DrawingForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (mouseClicked)
        {
            mouseCords(e);  
        }
    }
    //get mouse coordinates
    private void mouseCords(MouseEventArgs e)
    {
        this.x = e.X ;
        this.y = e.Y;
    }
    // Trigger the Paint event to keep drawing
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (mouseClicked)
        {
            this.Invalidate();  
        }
    }

    // Draw all non-zero points
    private void Init(object sender, PaintEventArgs e, int pointSize)
    {
        Graphics g = e.Graphics;
        for (int row = 0; row < arr.GetLength(0); row++)
        {
            for (int col = 0; col < arr.GetLength(1); col++)
            {
                if (arr[row, col] > 0) 
                {
                    using (Brush brush = new SolidBrush(Color.FromArgb(this.red, this.green, this.blue)))
                    {
                        g.FillEllipse(brush, row - pointSize / 2, col - pointSize / 2, pointSize, pointSize);
                    }
                    if (this.green > 244)
                    {
                        this.green = 0;
                    }
                    else this.green += 1;

                    if (this.red > 244)
                    {
                        this.red = 0;
                    }
                    else this.red += 1;

                    if (this.blue > 244)
                    {
                        this.blue = 0;
                    }
                    else this.blue += 1;
                }
            }
        }
    }

    //Create a point
    private void Gargir(object sender, PaintEventArgs e, int[,] arr, int pointSize)
    {
        Graphics g = e.Graphics;
        int row = this.x;
        for (int col = 0; col < arr.GetLength(1); col++)
        {
            if (arr[row, col] == 1)
            {
                Brush brush = Brushes.Black;
                g.FillEllipse(brush, row - pointSize / 2, col - pointSize / 2, pointSize, pointSize);
                if (col < 550)
                {
                    arr[row, col + 5] = 1;
                    arr[row, col] = 2;
                }
                break;
            }
        }
    }
    //clear privious points
    private void Clear(object sender, PaintEventArgs e, int[,] arr, int pointSize)
    {
        Graphics g = e.Graphics;
        int row = this.x;
        for (int col = 0; col < arr.GetLength(1); col++)
        {
            if (arr[row, col] == 2)
            {
                Brush backgroundBrush = new SolidBrush(this.BackColor);
                g.FillEllipse(backgroundBrush, row - pointSize / 2, col - pointSize / 2, pointSize, pointSize);
                arr[row, col] = 0;
            }
        }
    }
    //Main function
    private void DrawingForm_Paint(object sender, PaintEventArgs e)
    {
        if (this.mouseClicked)
        {
            int[,] arr = this.arr;
            Graphics g = e.Graphics;
            int r = this.x;
            int c = this.y;

            arr[r, c] = 1;
            int pointSize = 3;
            Init(sender, e, pointSize);
            while (c < 595 && arr[r, c + 5] != 1)
            {
                Gargir(sender, e, arr, pointSize);
                Clear(sender, e, arr, pointSize);
                c = c + 5;
            }
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new DrawingForm());
    }
}
