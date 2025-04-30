using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public static class FormUtils
    {
        public static void HandleFormClosing(object sender, FormClosingEventArgs e)
        {
            int visibleForms = Application.OpenForms.Cast<Form>().Count(f => f.Visible);

            if (visibleForms <= 1)
            {
                Application.Exit();
            }
        }


    }

}
