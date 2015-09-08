using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public class Graph
    {
        
            //所有頂點
            List<Vertex> vertexs = new List<Vertex>();
            //記錄圖形邊值為0代表沒有邊連接,值為1代表有邊連接
            int[,] graphEdges;
            //兩個頂點連接的所有路徑
            List<string> allPath = new List<string>();

            public Graph()
            {
            }

            /// <summary>
            /// 加入一個頂點
            /// </summary>
            /// <param name="value"></param>
            public void addVertex(string value)
            {
                Vertex vertex = new Vertex(value.Trim());
                vertexs.Add(vertex);
            }

            /// <summary>
            /// 加入多個頂點並以,作為區隔
            /// </summary>
            /// <param name="values"></param>
            public void addVertexs(string values)
            {
                foreach (string value in values.Split(','))
                {
                    Vertex vertex = new Vertex(value.Trim());
                    vertexs.Add(vertex);
                }
            }

            /// <summary>
            /// 當完成加入頂點初始化圖形
            /// </summary>
            public void compeleteAddVertexs()
            {
                int vertexsSize = vertexs.Count;
                //初始化圖形
                graphEdges = new int[vertexsSize, vertexsSize];
                for (int i = 0; i < vertexsSize; i++)
                {
                    for (int j = 0; j < vertexsSize; j++)
                    {
                        graphEdges[i, j] = 0;
                    }
                }
            }

            /// <summary>
            /// 對圖形加入邊
            /// </summary>
            /// <param name="vertexOne"></param>
            /// <param name="vertexTwo"></param>
            public void addEdge(string vertexOne, string vertexTwo)
            {
                int vtOneindex = vertexs.FindIndex(
                    delegate(Vertex vt)
                    {
                        if (vt.value == vertexOne)
                            return true;
                        return false;
                    });
                int vtTwoindex = vertexs.FindIndex(
                    delegate(Vertex vt)
                    {
                        if (vt.value == vertexTwo)
                            return true;
                        return false;
                    });
                graphEdges[vtOneindex, vtTwoindex] = 1;
                //graphEdges[vtTwoindex, vtOneindex] = 1;
            }

            /// <summary>
            /// 印出使用者圖形所有的邊(檢查所輸入邊是否有誤)
            /// </summary>
            public void printAllEdge()
            {
                int vertexsSize = vertexs.Count;
                for (int i = 0; i < vertexsSize; i++)
                {
                    for (int j = 0; j < vertexsSize; j++)
                    {
                        //代表兩個頂點有邊
                        if (graphEdges[i, j] == 1)
                        {
                            Console.WriteLine("{0}{1}  ", vertexs.ElementAt(i).value, vertexs.ElementAt(j).value);
                        }
                    }
                }
            }

            /// <summary>
            /// 尋找所有兩個頂點間的路徑
            /// </summary>
            /// <param name="vertexOne"></param>
            /// <param name="vertexTwo"></param>
            public void searchAllPathInTwoVertex(string vertexOne, string vertexTwo)
            {
                List<Vertex> copyVertexs = copyVertexList(vertexs);
                int[,] copyGraph = (int[,])graphEdges.Clone();
                allPath = searchAllPathInTwoVertex(vertexOne, vertexTwo, copyGraph, copyVertexs);
            }

            /// <summary>
            /// 尋找所有兩個頂點間的路徑的子迴圈程式
            /// </summary>
            /// <param name="vertexOne"></param>
            /// <param name="vertexTwo"></param>
            /// <param name="subgraaph"></param>
            /// <param name="subVertexs"></param>
            private List<string> searchAllPathInTwoVertex(string vertexOne, string vertexTwo, int[,] subGraphEdges, List<Vertex> subVertexs)
            {
                //取得vertexOn index位置
                int vtOneindex = subVertexs.FindIndex(
                    delegate(Vertex vt)
                    {
                        if (vt.value == vertexOne)
                            return true;
                        return false;
                    });

                //暫存路徑
                List<string> tempPathList = new List<string>();
                List<Vertex> copyVertexs = copyVertexList(subVertexs);
                //設定該頂點被使用過
                copyVertexs.ElementAt(vtOneindex).isSearch = true;
                //如果兩頂點一樣代表走到底無需在遞迴
                if (vertexOne.Equals(vertexTwo))
                {
                    tempPathList.Add(vertexTwo);
                }
                else
                {
                    for (int i = 0; i < subVertexs.Count; i++)
                    {
                        if (subGraphEdges[vtOneindex, i] == 1 && !subVertexs.ElementAt(i).isSearch)
                        {
                            //走過的邊要設成0
                            subGraphEdges[vtOneindex, i] = 0;
                            //複製邊複本
                            int[,] copyGraph = (int[,])subGraphEdges.Clone();
                            List<string> subTempList = searchAllPathInTwoVertex(vertexs.ElementAt(i).value, vertexTwo, copyGraph, copyVertexs);
                            if (subTempList != null)
                            {
                                foreach (string temp in subTempList)
                                {
                                    tempPathList.Add(vertexOne + ";" + temp);
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                }
                return tempPathList;
            }

            /// <summary>
            /// 製作copyVertexlist複本
            /// </summary>
            /// <param name="vertexsList"></param>
            /// <returns></returns>
            private List<Vertex> copyVertexList(List<Vertex> vertexsList)
            {
                List<Vertex> copyVertexsList = new List<Vertex>();
                foreach (Vertex vertex in vertexsList)
                {
                    Vertex copyVertex = new Vertex();
                    copyVertex.isSearch = vertex.isSearch;
                    copyVertex.value = vertex.value;
                    copyVertexsList.Add(copyVertex);
                }
                return copyVertexsList;
            }

            /// <summary>
            /// 印出所有路徑
            /// </summary>
            public void printAllPath()
            {
                Console.WriteLine("共有幾條{0}路徑", allPath.Count);

                if (allPath.Count != 0)
                {
                    foreach (string tempStr in allPath)
                    {
                        Console.WriteLine(tempStr);
                    }
                }
            }
            public List<string> returnAllPath()
            {
                if (allPath.Count != 0)
                {
                    return allPath;
                }
                return allPath;
            }
        
    }
}