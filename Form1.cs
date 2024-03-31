namespace NT106_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            DemoFirebase demoFirebase = new DemoFirebase();
            demoFirebase.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            DemoWebservice demoWebservice = new DemoWebservice();
            demoWebservice.ShowDialog();
            this.Show();
        }
    }
}
