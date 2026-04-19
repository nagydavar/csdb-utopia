using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Map {
   
        //private Graph g;
        private Dictionary<Coordinate, HashSet<Coordinate>> map = new();
        Dictionary<Coordinate, Dictionary<Coordinate,Tuple<Coordinate?, int>>> pathForAllPair =  new();
        Dictionary<Coordinate, Dictionary<Coordinate,bool>> adjMatrix = new ();
        //Dictionary<Tuple<Coordinate, Coordinate>, Section> preCalculatedSections = new();

        public Navigation GetNavigation(Coordinate start, Coordinate end)
        {
                return new Navigation(start, end, this);
        }
        private HashSet<Coordinate> getNeighbors(Coordinate coord)
        {
                HashSet<Coordinate> n = coord.GetAllNeighbors();
                n.RemoveWhere(c => !adjMatrix.ContainsKey(c)/* && !adjMatrix[coord][c]*/);
                return n;
        }

        public Coordinate? Step(Coordinate start, Coordinate end)
        {
                return pathForAllPair[start][end].Item1;
        }

        public void DeleteRoad(Coordinate c)
        {
                HashSet<Coordinate> neighbors = getNeighbors(c);
                foreach (Coordinate n in neighbors)
                {
                        adjMatrix[c][n] = false;
                        adjMatrix[n][c] = false;
                        UpdatePathDeleteRoad(c, n);
                }
        }
        
        public Coordinate GetNearest(Coordinate start, List<Coordinate> ends)
        {
                
                int minDist = GetDistance(start, ends[0]);
                Coordinate minCoord = ends[0];
                for (int i = 1; i < ends.Count; i++)
                {
                        int dist = GetDistance(start, ends[i]);
                        if (minDist > dist)
                        {
                                minDist = dist;
                                minCoord = ends[i];
                        }
                }

                return minCoord;
        }

        public void BuildRoad(Coordinate coord)
        {
                IEnumerable<Coordinate> affectedCoords = getNeighbors(coord);

                Dictionary<Coordinate, bool> newRow = new();
                pathForAllPair.TryAdd(coord, new Dictionary<Coordinate, Tuple<Coordinate?, int>>());
                foreach (Coordinate nextCoord in adjMatrix.Keys)
                {
                        bool isNeighbor = affectedCoords.Contains(nextCoord);
                        
                        
                        if (!adjMatrix[nextCoord].TryAdd(coord, isNeighbor))
                                adjMatrix[nextCoord][coord] = isNeighbor;

                        int newDist = isNeighbor ? 1 : int.MaxValue;
                        Coordinate? parent1 = isNeighbor ? nextCoord : null;
                        Coordinate? parent2 = isNeighbor ? coord : null;
                        Tuple<Coordinate?, int> toNext = new Tuple<Coordinate?, int>(parent1, newDist);
                        if (!pathForAllPair[coord].TryAdd(nextCoord, toNext))
                                pathForAllPair[coord][nextCoord] = toNext;
                        Tuple<Coordinate?, int> toThis = new Tuple<Coordinate?, int>(parent2, newDist);
                        if (!pathForAllPair[nextCoord].TryAdd(coord, toThis))
                                pathForAllPair[nextCoord][coord] = toThis;
                        
                        newRow.Add(nextCoord, isNeighbor);
                }

                if (!adjMatrix.TryAdd(coord, newRow))
                        adjMatrix[coord] = newRow;
                if (!adjMatrix[coord].TryAdd(coord, false))
                        adjMatrix[coord][coord] = false;
                if (!pathForAllPair[coord].TryAdd(coord, new Tuple<Coordinate?, int>(null, 0)))
                        pathForAllPair[coord][coord] = new Tuple<Coordinate?, int>(null, 0);
                foreach (var n in affectedCoords)
                {
                        UpdatePathBuildRoad(coord, n); 
                }
        }

        /*
        private void AddRoad(Coordinate c1, Coordinate c2)
        {
                if (!c1.GetAllNeighbors().Contains(c2)) throw new ArgumentException();

                map[c1].Add(c2);
                map[c2].Add(c1);



        }
        private Section? getPreCalculated(Coordinate c1, Coordinate c2)
        {
                Tuple<Coordinate, Coordinate> ab = new Tuple<Coordinate, Coordinate>(c1, c2);
                Tuple<Coordinate, Coordinate> ba = new Tuple<Coordinate, Coordinate>(c2, c1);

                if (preCalculatedSections.TryGetValue(ab, out var precalculated1))
                        return precalculated1;
                else if (preCalculatedSections.TryGetValue(ba, out var preCalculated2))
                        return preCalculated2;
                return null;
        }


        private async Task recalculate(Coordinate start, Coordinate end)
        {

                Section? preCalculated = getPreCalculated(start, end);
                if (preCalculated is null) throw new ArgumentException();

                preCalculatedSections[new Tuple<Coordinate, Coordinate>(start, end)] = await calculateSection(start, end);
        }

        private async Task<Section> calculateSection(Coordinate start, Coordinate end)
        {
                Coordinate? c = pathForAllPair[start][end].Item1;
                if (c is null) throw new ArgumentException();
                List<Coordinate> path = new();
                path.Add(c.Value);
                while (c != start)
                {

                        c = pathForAllPair[c.Value][end].Item1;

                        if (c is null) throw new ArgumentException();
                        path.Add(c.Value);
                }
        }

        public async Task<Section> calculate(Coordinate start, Coordinate end)
        {
                Section? preCalculated = getPreCalculated(start, end);
                if (preCalculated is not null) return preCalculated;
                Section section = await calculateSection(start, end);

                preCalculatedSections.Add(new Tuple<Coordinate, Coordinate>(start, end), section);

                return section;
        }

        public Section calculateOld(Coordinate c1, Coordinate c2)
        {
                //Naive: BFS


                if (!map.ContainsKey(c1) || !map.ContainsKey(c2)) throw new ArgumentException();
                Dictionary<Coordinate, Coordinate> parent = new();
                Dictionary<Coordinate, int> distance = new();
                Queue<Coordinate> S = new Queue<Coordinate>();
                S.Enqueue(c1);
                while (S.Count > 0)
                {
                        Coordinate road = S.Dequeue();
                        foreach (Coordinate neighbor in map[road])
                        {
                                if (!distance.ContainsKey(neighbor))
                                {
                                        distance.Add(neighbor, distance[road] + 1);
                                        parent.Add(neighbor, road);
                                        S.Enqueue(neighbor);
                                }

                        }

                }
        }
           public Path calculate(List<Coordinate> coords)
                {
                        List<Section> sections = new List<Section>(coords.Count-1);
                        for (int i = 0; i < coords.Count-1; i++)
                        {
                                Section? s = calculate(coords[i], coords[i + 1]);
                                if (s is null) throw new ArgumentException();
                                sections.Add(s);
                        }
                        Path path = new Path(sections);
                }
*/
        private int GetDistance(Coordinate c1, Coordinate c2) => pathForAllPair[c1][c2].Item2;
        private Coordinate? GetParent(Coordinate c1, Coordinate c2) => pathForAllPair[c1][c2].Item1;
        
        private void UpdatePathDeleteRoad(Coordinate i, Coordinate j)
        {
                
                adjMatrix[i][j] = false;
                adjMatrix[j][i] = false;
                HashSet<Tuple<Coordinate, Coordinate >> affectedPairs = new();
                HashSet<Coordinate> jAdj = new HashSet<Coordinate>(); //i ből ide lehet eljutni legrövidebben
                HashSet<Coordinate> iAdj = new HashSet<Coordinate>(); //j be innen lehet eljutni legrövidebben

                foreach (Coordinate c in pathForAllPair.Keys)
                {
                        if (pathForAllPair[c][i].Item1 is not null)
                                iAdj.Add(c);
                }
                
                foreach (Coordinate c in pathForAllPair[j].Keys)
                {
                        if (pathForAllPair[j][c].Item1 is not null)
                                jAdj.Add(c);
                }

                HashSet<Coordinate> touchIJ = iAdj.Union(jAdj).ToHashSet();
                touchIJ.Add(i);
                touchIJ.Add(j);
                foreach (Coordinate a in touchIJ)
                {
                        foreach (Coordinate b in touchIJ)
                        {
                                Coordinate? parent = adjMatrix[a][b] ? b : null;
                                int dist = a == b ? 0 : adjMatrix[a][b] ? 1 : int.MaxValue;
                                pathForAllPair[a][b] = new Tuple<Coordinate?, int>(parent, dist);
                        }
                }

                foreach (Coordinate k in pathForAllPair.Keys)
                {
                        foreach (Coordinate c in touchIJ)
                        {
                                foreach (Coordinate d in touchIJ)
                                {
                                        int distCD = GetDistance(c, d);
                                        int distCKD = GetDistance(c, k) + GetDistance(k, d);
                                        if (GetParent(c, k).HasValue && GetParent(k, d).HasValue && distCD > distCKD)
                                        {
                                                Coordinate? newParent = pathForAllPair[k][d].Item1;
                                                Tuple<Coordinate?, int> newDistParent = new(newParent, distCKD);
                                                pathForAllPair[c][d] = newDistParent;

                                        }
                                }
                        }
                }
                
        }
        private void UpdatePathBuildRoad(Coordinate i, Coordinate j)
        {
                adjMatrix[i][j] = true;
                adjMatrix[j][i] = true;
                foreach (Coordinate a in pathForAllPair.Keys)
                {
                        foreach (Coordinate b in pathForAllPair.Keys)
                        {
                                int distAIJB = GetDistance(a, i) + GetDistance(j, b) + 1;
                                if ((a == i || GetParent(a, i).HasValue ) && (GetParent(j, b).HasValue || j == b) && GetDistance(a, b) > distAIJB)
                                {
                                                
                                        Coordinate? newParent = a == i ? j: pathForAllPair[a][i].Item1;
                                        pathForAllPair[a][b] = new Tuple<Coordinate?, int>(newParent, distAIJB);
                                      
                                }
                                int distAJIB = GetDistance(a, j) + GetDistance(i, b) + 1;
                                if ((a == j || GetParent(a, j).HasValue ) && (GetParent(i, b).HasValue || i == b) && GetDistance(a, b) > distAJIB)
                                {
                                                
                                        Coordinate? newParent = a == j ? j: pathForAllPair[a][j].Item1;
                                        pathForAllPair[a][b] = new Tuple<Coordinate?, int>(newParent, distAJIB);
                                      
                                }
                        }
                }

                
        }
        

        private void floydWarshallPrepare(HashSet<Coordinate> coords)
        {

                foreach (Coordinate i in coords)
                {
                        pathForAllPair.TryAdd(i, new Dictionary<Coordinate, Tuple<Coordinate?, int>>());
                        adjMatrix.TryAdd(i, new Dictionary<Coordinate, bool>());
                        HashSet<Coordinate> neighbors = i.GetAllNeighbors();
                        foreach (Coordinate j in coords)
                        {
                                Coordinate? parent;
                                int dist;
                                
                                if (neighbors.Contains(j))
                                {
                                        parent = j;
                                        dist = 1;
                                        adjMatrix[i][j] = true;
                                }
                                else if (i == j)
                                {
                                        parent = null;
                                        dist = 0;
                                        
                                        adjMatrix[i][j] = false;
                                }
                                else
                                {
                                        parent = null;
                                        dist = int.MaxValue;
                                        
                                        adjMatrix[i][j] = false;
                                }
                                Tuple<Coordinate?, int> pair = new Tuple<Coordinate?, int>(parent, dist);
                                pathForAllPair[i].Add(j, pair);
                                
                        }
                }

                foreach (Coordinate k in pathForAllPair.Keys)
                {
                        foreach (Coordinate i in pathForAllPair.Keys)
                        {
                                foreach (Coordinate j in pathForAllPair.Keys)
                                {
                                        int distIJ = GetDistance(i, j);
                                        int distIKJ = GetDistance(i, k) + GetDistance(k, j);
                                        if (GetParent(i, k).HasValue && GetParent(k, j).HasValue && distIJ > distIKJ)
                                        {
                                                Coordinate? newParent = pathForAllPair[i][k].Item1;
                                                Tuple<Coordinate?, int> a = new(newParent, distIKJ);
                                                pathForAllPair[i][j] = a;

                                        }
                                }
                        }
                }

        }

     

        public Map(HashSet<Coordinate> coords)
        {
               floydWarshallPrepare(coords);
        }
};
