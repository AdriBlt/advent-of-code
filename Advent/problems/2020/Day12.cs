using System;
using System.Linq;

namespace Advent2020
{
    class Day12: BaseDay2020
    {
        private (char direction, int distance)[] instructions;

        private long shipPostionNS = 0;
        private long shipPostionEW = 0;
        private char shipDirection = 'E';

        private long waypointPostionNS = 0;
        private long waypointPostionEW = 0;

        public Day12(): base(12) {
            this.instructions = this.lines.Select(line => {
                return (line[0], int.Parse(line.Substring(1)));
            }).ToArray();
        }

        public override long QuestionA()
        {
            return this.ComputeShipFinalPosiitonDistance(this.ApplyInstructionToShip);
        }

        public override long QuestionB()
        {
            return this.ComputeShipFinalPosiitonDistance(this.ApplyInstructionToWaypoint);
        }

        private long ComputeShipFinalPosiitonDistance(Action<char, int> applyInstruction)
        {
            this.Reset();
            foreach ((char direction, int distance) in this.instructions)
            {
                applyInstruction(direction, distance);
            }

            return Math.Abs(shipPostionNS) + Math.Abs(shipPostionEW);
        }

        private void Reset()
        {
            this.shipPostionNS = 0;
            this.shipPostionEW = 0;
            this.shipDirection = 'E';

            this.waypointPostionNS = 1;
            this.waypointPostionEW = 10;
        }

        #region Question A
        private void ApplyInstructionToShip(char direction, int distance)
        {
            switch (direction)
            {
                case 'N':
                    this.shipPostionNS += distance;
                    break;
                case 'S':
                    this.shipPostionNS -= distance;
                    break;
                case 'E':
                    this.shipPostionEW += distance;
                    break;
                case 'W':
                    this.shipPostionEW -= distance;
                    break;
                case 'L':
                    this.TurnShip(distance);
                    break;
                case 'R':
                    this.TurnShip(-distance);
                    break;
                case 'F':
                    this.ApplyInstructionToShip(shipDirection, distance);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void TurnShip(int angle)
        {
            int currentAngle;
            switch (this.shipDirection)
            {
                case 'E':
                    currentAngle = 0;
                    break;
                case 'N':
                    currentAngle = 90;
                    break;
                case 'W':
                    currentAngle = 180;
                    break;
                case 'S':
                    currentAngle = 270;
                    break;
                default:
                    throw new ArgumentException();
            }

            currentAngle = (currentAngle + angle + 360) % 360;

            switch (currentAngle)
            {
                case 0:
                    this.shipDirection = 'E';
                    break;
                case 90:
                    this.shipDirection = 'N';
                    break;
                case 180:
                    this.shipDirection = 'W';
                    break;
                case 270:
                    this.shipDirection = 'S';
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        #endregion

        #region Question B
        private void ApplyInstructionToWaypoint(char direction, int distance)
        {
            switch (direction)
            {
                case 'N':
                    this.waypointPostionNS += distance;
                    break;
                case 'S':
                    this.waypointPostionNS -= distance;
                    break;
                case 'E':
                    this.waypointPostionEW += distance;
                    break;
                case 'W':
                    this.waypointPostionEW -= distance;
                    break;
                case 'L':
                    this.TurnWaypoint(distance);
                    break;
                case 'R':
                    this.TurnWaypoint(-distance);
                    break;
                case 'F':
                    this.shipPostionNS += distance * this.waypointPostionNS;
                    this.shipPostionEW += distance * this.waypointPostionEW;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void TurnWaypoint(int angle)
        {
            long turningAngle = (angle + 360) % 360;
            long tmpNS = this.waypointPostionNS;
            switch (turningAngle)
            {
                case 0:
                    break;
                case 90:
                    this.waypointPostionNS = this.waypointPostionEW;
                    this.waypointPostionEW = -tmpNS;
                    break;
                case 180:
                    this.waypointPostionNS = -this.waypointPostionNS;
                    this.waypointPostionEW = -this.waypointPostionEW;
                    break;
                case 270:
                    this.waypointPostionNS = -this.waypointPostionEW;
                    this.waypointPostionEW = tmpNS;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        #endregion
    }
}
