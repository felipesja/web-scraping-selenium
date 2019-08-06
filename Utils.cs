using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingSelenium {
    public class Utils {

        public static Boolean IsFileOpen(String filePathTest) {
            try {
                using (Stream stream = new FileStream(filePathTest, FileMode.Open)) {
                    return false;
                }
            }
            catch {
                return true;
            }
        }
    }
}
