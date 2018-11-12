using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MonteCarlo
{
    class Automat2D
    {
        private int[,] cells;
        private int ibound;
        private int jbound;
        private string wb;
        private string sasiedztwo;
        private volatile bool work = false;
        PictureBox pictureBox;
        Bitmap bitmap;
        private int N, M;
        private List<Color> colors;
        private string wybor;

        public int[,] Cells
        {
            get
            {
                return cells;
            }

            set
            {
                cells = value;
            }
        }

        public int Ibound
        {
            get
            {
                return ibound;
            }

            set
            {
                ibound = value;
            }
        }

        public int Jbound
        {
            get
            {
                return jbound;
            }

            set
            {
                jbound = value;
            }
        }

        public string Wb { get => wb; set => wb = value; }
        public string Sasiedztwo { get => sasiedztwo; set => sasiedztwo = value; }
        public int N1 { get => N; set => N = value; }
        public int M1 { get => M; set => M = value; }
        public List<Color> Colors { get => colors; set => colors = value; } 
        public void stop() { work = false; }

        

        public Automat2D(int N, int M, string wb, PictureBox pictureBox, Bitmap bitmap,int colors_number, List<Color> colors,string wybor)
        {
            cells = new int[N, M];
            this.pictureBox = pictureBox;
            this.bitmap = bitmap;
            this.wb = wb;
            this.N = N;
            this.M = M;
            this.colors = colors;
            this.wybor = wybor;
            ibound = cells.GetUpperBound(0);
            jbound = cells.GetUpperBound(1);
            



            Random rnd = new Random(DateTime.Now.Second);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    Cells[i, j] = colors[rnd.Next(0, colors_number)].ToArgb();
                    bitmap.SetPixel(i, j,Color.FromArgb(Cells[i,j]));
                }

            pictureBox.Image = bitmap;
        }

        private void Refresh(PictureBox pictureBox, Bitmap bitmap, int N, int M)
        {
            /////ustawianie poczatkowej bitmapy
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(Cells[i, j]));
                }
            pictureBox.Invoke(new Action(() => pictureBox.Image = bitmap));
            /////////////////////////////////////
        }

        //public void Iterate(Action oniterate)
        public void Iterate()
        {

            cells = GetnextIterationCells(wb,wybor);
            // oniterate();
        }


        private int[,] GetnextIterationCells(string wb, string wybor)
        {

            int[,] newCells = new int[cells.GetLength(0), cells.GetLength(1)];
            Random random = new Random(DateTime.Now.Millisecond);

            work = true;
            if (wybor == "Losowy")
            {
                //work = true;
                while (work)
                {
                    int i = random.Next(0, newCells.GetLength(0));
                    int j = random.Next(0, newCells.GetLength(1));
                    newCells[i, j] = getCellstateRandom(getNeighboursMoore(i, j, wb), i, j);
                    pictureBox.Invoke(new Action(() =>
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(newCells[i, j]));
                        pictureBox.Image = bitmap;
                    }
                    ));
                }
            }else if(wybor == "Sąsiedztwo")
            {
                while (work)
                {
                    int i = random.Next(0, newCells.GetLength(0));
                    int j = random.Next(0, newCells.GetLength(1));
                    newCells[i, j] = getCellstateNeighbours(getNeighboursMoore(i, j, wb), i, j);
                    pictureBox.Invoke(new Action(() =>
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(newCells[i, j]));
                        pictureBox.Image = bitmap;
                    }
                    ));
                }
            }
            return newCells;
        }
        private List<int> getNeighboursMoore(int i, int j, string wb)
        {
            List<int> neighbours = new List<int>();


            if (wb == "periodyczne")
            {
                if (i == 0 && j != 0 && j != jbound)
                {
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[ibound, k]);
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[i + 1, k]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                }
                else if (i == ibound && j != 0 && j != jbound)
                {
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[0, k]);
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[ibound - 1, k]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                }
                else if (j == 0 && i != 0 && i != ibound)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, jbound]);
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, j + 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else if (j == jbound && i != 0 && i != ibound)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, 0]);
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else if (i == 0 && j == 0)
                {
                    neighbours.Add(cells[i, j + 1]);
                    neighbours.Add(cells[i + 1, j + 1]);
                    neighbours.Add(cells[i + 1, j]);
                    neighbours.Add(cells[i, jbound]);
                    neighbours.Add(cells[i + 1, jbound]);
                    neighbours.Add(cells[ibound, j]);
                    neighbours.Add(cells[ibound, j + 1]);
                    neighbours.Add(cells[ibound, jbound]);

                }
                else if (i == ibound && j == 0)
                {
                    neighbours.Add(cells[i - 1, j + 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i, j + 1]);
                    neighbours.Add(cells[0, j]);
                    neighbours.Add(cells[0, j + 1]);
                    neighbours.Add(cells[i - 1, jbound]);
                    neighbours.Add(cells[i, jbound]);
                    neighbours.Add(cells[0, jbound]);
                }
                else if (i == ibound && j == jbound)
                {
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i - 1, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i - 1, 0]);
                    neighbours.Add(cells[i, 0]);
                    neighbours.Add(cells[0, j - 1]);
                    neighbours.Add(cells[0, jbound]);
                    neighbours.Add(cells[0, 0]);
                }
                else if (i == 0 && j == jbound)
                {
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i + 1, j - 1]);
                    neighbours.Add(cells[i + 1, j]);
                    neighbours.Add(cells[ibound, j - 1]);
                    neighbours.Add(cells[ibound, j]);
                    neighbours.Add(cells[i, 0]);
                    neighbours.Add(cells[i + 1, 0]);
                    neighbours.Add(cells[ibound, 0]);

                }
                else
                {
                    neighbours.Add(cells[i - 1, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i - 1, j + 1]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                    neighbours.Add(cells[i + 1, j - 1]);
                    neighbours.Add(cells[i + 1, j]);
                    neighbours.Add(cells[i + 1, j + 1]);
                }

            }

            if (wb == "nieperiodyczne")
            {
                if (i == 0 && j != 0 && j != jbound)
                {
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[i + 1, k]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                }
                else if (i == ibound && j != 0 && j != jbound)
                {
                    for (int k = j - 1; k <= j + 1; k++)
                        neighbours.Add(cells[ibound - 1, k]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                }
                else if (j == 0 && i != 0 && i != ibound)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, j + 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else if (j == jbound && i != 0 && i != ibound)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                        neighbours.Add(cells[k, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else if (i == 0 && j == 0)
                {
                    neighbours.Add(cells[i, j + 1]);
                    neighbours.Add(cells[i + 1, j + 1]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else if (i == ibound && j == 0)
                {
                    neighbours.Add(cells[i - 1, j + 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i, j + 1]);
                }
                else if (i == ibound && j == jbound)
                {
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i - 1, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                }
                else if (i == 0 && j == jbound)
                {
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i + 1, j - 1]);
                    neighbours.Add(cells[i + 1, j]);
                }
                else
                {
                    neighbours.Add(cells[i - 1, j - 1]);
                    neighbours.Add(cells[i - 1, j]);
                    neighbours.Add(cells[i - 1, j + 1]);
                    neighbours.Add(cells[i, j - 1]);
                    neighbours.Add(cells[i, j + 1]);
                    neighbours.Add(cells[i + 1, j - 1]);
                    neighbours.Add(cells[i + 1, j]);
                    neighbours.Add(cells[i + 1, j + 1]);
                }
            }

            return neighbours;
        }
      
        public int getCellstateRandom(List<int> neighbours, int i, int j)
        {
            Random rnd = new Random(DateTime.Now.Second);
            int Energy_prev;
            int Energy_next;
            int color_toChange;
            //int buff = 0;
            Energy_prev = (from x in neighbours
                           where x != Cells[i, j]
                           select x).Count();

            if (Energy_prev == 0)
                return Cells[i, j];
            while (true)
            {
                color_toChange = colors[rnd.Next(0, colors.Count())].ToArgb();
                //color_toChange = colors[buff].ToArgb();

                Energy_next = (from x in neighbours
                               where x != color_toChange
                               select x).Count();

                if (Energy_next <= Energy_prev)
                {
                    Cells[i, j] = color_toChange;
                    break;
                }

                //buff++;
            }
            return Cells[i, j];
        }

        public int getCellstateNeighbours(List<int> neighbours, int i, int j)
        {
            Random rnd = new Random(DateTime.Now.Second);
            int Energy_prev;
            int Energy_next;
            int color_toChange;
            int size = neighbours.Count();
            Energy_prev = (from x in neighbours
                           where x != Cells[i, j]
                           select x).Count();

            if (Energy_prev == 0)
                return Cells[i, j];
            while (true)
            {
                color_toChange = neighbours[rnd.Next(0, size)];
                //color_toChange = colors[buff].ToArgb();

                Energy_next = (from x in neighbours
                               where x != color_toChange
                               select x).Count();

                if (Energy_next <= Energy_prev)
                {
                    Cells[i, j] = color_toChange;
                    break;
                }
            }
            return Cells[i, j];
        }
    }
}
