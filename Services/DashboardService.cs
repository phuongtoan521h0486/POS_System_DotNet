using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Order;

namespace POS_System_DotNet.Services
{
    public class DashboardService
    {
        private MyContext ct;
        public DashboardService(MyContext ct)
        {
            this.ct = ct;
        }

        public List<Order> getOrdersFromRange(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Order> orders;

            if (DateRangeChecker.IsToday(startDate, endDate))
            {
                orders = ct.Orders
                    .Where(o => o.OrderDate.Date >= DateTime.Today && o.OrderDate.Date < DateTime.Today.AddDays(1))
                    .ToList();
            }
            else if (DateRangeChecker.IsYesterday(startDate, endDate))
            {
                orders = ct.Orders
                    .Where(o => o.OrderDate.Date >= DateTime.Today.AddDays(-1) && o.OrderDate.Date < DateTime.Today)
                    .ToList();
            }
            else if (DateRangeChecker.IsLast7Days(startDate, endDate))
            {
                orders = ct.Orders
                    .Where(o => o.OrderDate.Date >= DateTime.Today.AddDays(-6) && o.OrderDate.Date < DateTime.Today.AddDays(1))
                    .ToList();
            }
            else if (DateRangeChecker.IsThisMonth(startDate, endDate))
            {
                DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                orders = ct.Orders
                    .Where(o => o.OrderDate.Date >= firstDayOfMonth && o.OrderDate.Date <= lastDayOfMonth)
                    .ToList();
            }
            else
            {
                orders = ct.Orders
                    .Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate)
                    .ToList();
            }


            return orders.ToList();
        }

        public int getSize(DateTime? startDate, DateTime? endDate)
        {
            return ct.Orders.Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate).Count();
        }

        public int getTotalProduct(DateTime? startDate, DateTime? endDate)
        {
            List<Order> orders = ct.Orders.Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate).ToList();
            int total = 0;
            foreach(Order order in orders)
            {
                List<OrderDetail> details = ct.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                foreach(OrderDetail detail in details)
                {
                    total += detail.Quantity;
                }
            }
            return total;
        }

        public double getRevenue(DateTime? startDate, DateTime? endDate)
        {
           return ct.Orders
                .Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate)
                .Select(x =>x.TotalAmount)
                .ToList().Sum();

        }

        public double getProfit(DateTime? startDate, DateTime? endDate)
        {
            List<Order> orders = ct.Orders.Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate).ToList();
            double total = 0;
            foreach (Order order in orders)
            {
                List<OrderDetail> details = ct.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                foreach (OrderDetail detail in details)
                {
                    total += ct.Products.FirstOrDefault(x => x.ProductId == detail.ProductId).ImportPrice;
                }
            }

            return getRevenue(startDate, endDate) - total;

        }

        public List<double> getBarChart(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.Date;
            endDate ??= DateTime.Today.Date;

            if (startDate == endDate)
            {
                var barChartData = ct.Orders
                .Where(o => o.OrderDate.Date == startDate)
                .GroupBy(o => o.OrderDate.Date)

                .Select(group => new
                {
                    Date = group.Key,
                    TotalAmount = group.Sum(o => o.TotalAmount)
                })
                .OrderBy(entry => entry.Date)
                .ToList();

                var result = barChartData.Select(entry => entry.TotalAmount).ToList();
                return result;
            } else
            {
                var dateRange = GetAllDatesBetween(startDate.Value, endDate.Value);

                var barChartData = ct.Orders
                    .Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date < endDate)
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(group => new
                    {
                        Date = group.Key,
                        TotalAmount = group.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(entry => entry.Date)
                    .ToList();

                var result = dateRange
                    .GroupJoin(barChartData,
                        date => date,
                        chartData => chartData.Date,
                        (date, chartData) => new
                        {
                            Date = date,
                            TotalAmount = chartData.Sum(data => data.TotalAmount)
                        })
                    .OrderBy(entry => entry.Date)
                    .Select(entry => entry.TotalAmount)
                    .ToList();

                return result;
            }
        }
        private List<DateTime> GetAllDatesBetween(DateTime startDate, DateTime endDate)
        {
            var dateList = new List<DateTime>();
            DateTime currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                dateList.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dateList;
        }
    }
    public static class DateRangeChecker
    {
        public static bool IsToday(DateTime? startDate, DateTime? endDate)
        {
            return IsCustomRange(startDate, endDate, DateTime.Today, DateTime.Today);
        }

        public static bool IsYesterday(DateTime? startDate, DateTime? endDate)
        {
            return IsCustomRange(startDate, endDate, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1));
        }

        public static bool IsLast7Days(DateTime? startDate, DateTime? endDate)
        {
            DateTime sevenDaysAgo = DateTime.Today.AddDays(-6);
            return IsCustomRange(startDate, endDate, sevenDaysAgo, DateTime.Today);
        }

        public static bool IsThisMonth(DateTime? startDate, DateTime? endDate)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return IsCustomRange(startDate, endDate, firstDayOfMonth, lastDayOfMonth);
        }

        public static bool IsCustomRange(DateTime? startDate, DateTime? endDate, DateTime rangeStart, DateTime rangeEnd)
        {
            startDate ??= DateTime.MinValue;
            endDate ??= DateTime.MaxValue;

            return startDate.Value.Date >= rangeStart.Date && endDate.Value.Date <= rangeEnd.Date;
        }

    }
}
