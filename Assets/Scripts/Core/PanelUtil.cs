using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

	/// <summary>
	/// 界面管理工具
	/// </summary>
	public class PanelUtil
	{
		/// <summary>
		/// 记录打开的界面
		/// 数字最大的表示最近打开的界面，为0表示已经关闭了的
		/// </summary>
        private static List<BasePanel> dictOpenedPanel = new List<BasePanel>();
		/// <summary>
		/// 记录打开的界面
		/// </summary>
		/// <param name="panel"></param>
		public static void RecordOpenPanel(BasePanel panel)
		{
            if (dictOpenedPanel.Contains(panel))
			{
                return;
			}
			else
			{
				dictOpenedPanel.Add(panel);
			}
            //panel.transform.localPosition = Vector3.back * (PanelUtil.GetOpenPanelCount() * 500);
            TheLastOpenUgui = panel;
		}
        public static BasePanel TheLastOpenUgui;
		/// <summary>
		/// 记录关闭的界面
		/// </summary>
		/// <param name="panel"></param>
		public static void RecordClosePanel(BasePanel panel)
		{
            if (dictOpenedPanel.Contains(panel))
			{

				dictOpenedPanel.Remove(panel);
			}
            dictOpenedPanel.Sort((a, b) =>
                {
                    return a.transform.GetSiblingIndex() - b.transform.GetSiblingIndex();
                });
            //for (int i = 0; i < dictOpenedPanel.Count; i++)
            //{
            //    dictOpenedPanel[i].transform.localPosition = Vector3.back * (i * 2000);
            //}
		}
		/// <summary>
		/// 将所有已经记录为打开的界面关闭
		/// </summary>
		public static void CloseAllPanel()
		{
            while (dictOpenedPanel.Count > 0)
            { 
                dictOpenedPanel[0].Close();
            }
			dictOpenedPanel.Clear();
		}
        public static void CloseExcept(BasePanel p)
        {
            if (dictOpenedPanel.Contains(p))
            {
                while (dictOpenedPanel.Count > 1)
                {
                    if (dictOpenedPanel[0] == p)
                    {
                        dictOpenedPanel[1].Close();
                    }else
                        dictOpenedPanel[0].Close();
                }
                dictOpenedPanel.Clear();
            }
            else
                CloseAllPanel();
        }
		/// <summary>
		/// 清除所有记录
		/// </summary>
		public static void ClearRecord()
		{
			dictOpenedPanel.Clear();
		}
        public static bool PaneIsOpen(BasePanel p)
        {
            if (dictOpenedPanel.Contains(p))
                return true;
            return false;
        }
        public static bool AnyOpen()
        {
            return dictOpenedPanel.Count > 0;
        }
        public static int GetOpenPanelCount()
        {
            return dictOpenedPanel.Count;
        }
	}
