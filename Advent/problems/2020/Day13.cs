using System;
using System.Linq;

namespace Advent2020
{
    class Day13: BaseDay2020
    {
        private long arrivalTime;
        private (int id, int index)[] availableBuses;

        public Day13(): base(13)
        {
            this.arrivalTime = long.Parse(this.lines[0]);
            this.availableBuses = this.lines[1].Split(',')
                .Select((line, i) =>
                {
                    if (line == "x")
                    {
                        return (-1, -1);
                    }
                    else
                    {
                        return (int.Parse(line), i);
                    }
                })
                .Where(res => res.Item1 > -1)
                .ToArray();
        }

        public override long QuestionA()
        {
            (int id, long waiting) initValue = (-1, long.MaxValue);
            return this.availableBuses
                 .Select(bus =>
                     {
                         int id = bus.id;
                         long div = (long)Math.Ceiling(1.0 * this.arrivalTime / id);
                         long departureTime = div * id;
                         long waiting = departureTime - this.arrivalTime;
                         return (id, waiting);
                     })
                 .Aggregate(
                    initValue,
                    ((int id, long waiting) min, (int id, long waiting) next) => min.waiting < next.waiting ? min : next,
                    result => result.id * result.waiting
                );
        }

        public override long QuestionB()
        {
            long t = 1;
            long incr = 1;
            foreach ((int id, int index) in this.availableBuses)
            {
                while ((t + index) % id > 0) {
                    t += incr;
                }

                incr *= id;
            }
            
            return t;
        }
    }
}
