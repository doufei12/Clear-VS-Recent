using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clear_VS_Recent.Utils
{
    public class Json
    {
        public static JArray Deserialize(string strJson)
        {
            try
            {
                var json_object = JArray.Parse(strJson);
                return json_object;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
    }
}
