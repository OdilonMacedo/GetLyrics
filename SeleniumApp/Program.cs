using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumApp;

class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://www.google.com/search?q=tropa+do+mais+novo+letra&oq=tropa+do+mais+novo+letra&gs_lcrp=EgZjaHJvbWUqBwgAEAAYgAQyBwgAEAAYgAQyCAgBEAAYFhgeMggIAhAAGBYYHjIICAMQABgWGB7SAQgyNjA1ajBqN6gCALACAA&sourceid=chrome&ie=UTF-8");
        var paragrafos = driver.FindElements(By.XPath("//*[@id=\"kp-wp-tab-default_tab:kc:/music/recording_cluster:lyrics\"]/div[1]/div/div/div[2]/div/div/div/div/div[1]/div[2]"));

        foreach (var paragrafo in paragrafos)
        {
            Console.WriteLine();
            Console.WriteLine("***********Tropa do mais novo***********");
            Console.WriteLine();
            Console.WriteLine(paragrafo.Text);
            Console.WriteLine();
        }
    }
}
