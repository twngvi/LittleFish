
using System;
using System.Windows.Forms;

namespace App
{
    public class BaseForm : Form
    {
    //    protected override void OnFormClosing(FormClosingEventArgs e)
    //    {

    //        if (this.GetType().Name == "frmQuickAddAccount")
    //            return;

    //        DialogResult result = MessageBox.Show(
    //            "Bạn có muốn thoát chương trình?",
    //            "Xác nhận thoát",
    //            MessageBoxButtons.OKCancel,
    //            MessageBoxIcon.Question
    //        );
    //        if (result == DialogResult.OK)
    //        {
    //            // Cách 1 – thoát toàn bộ nhanh gọn
    //            Application.ExitThread();  // ✅ Ưu tiên dùng cách này

    //            // Cách 2 – đóng từng form (nếu muốn kiểm soát chặt hơn)
    //            /*
    //            foreach (Form frm in Application.OpenForms.Cast<Form>().ToList())
    //            {
    //                frm.Close(); // KHÔNG dùng nếu các form còn xử lý FormClosing
    //            }
    //            */
    //        }
    //        if (result == DialogResult.OK)
    //        {
    //            Application.ExitThread(); // Đóng toàn bộ app, không gây hỏi lại
    //        }

    //        if (result == DialogResult.OK)
    //        {
    //            Application.Exit(); // Thoát toàn bộ chương trình
    //        }
    //        else
    //        {
    //            e.Cancel = true;
    //        }
    //    }

    //    private void InitializeComponent()
    //    {
    //        this.SuspendLayout();
    //        // 
    //        // BaseForm
    //        // 
    //        this.ClientSize = new System.Drawing.Size(282, 253);
    //        this.Name = "BaseForm";
    //        this.Load += new System.EventHandler(this.BaseForm_Load);
    //        this.ResumeLayout(false);

    //    }

    //    private void BaseForm_Load(object sender, EventArgs e)
    //    {

    //    }
    }
}
