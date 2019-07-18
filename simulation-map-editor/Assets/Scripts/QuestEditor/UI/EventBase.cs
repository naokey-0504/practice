using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.QuestEditor
{
    public class EventBase : MonoBehaviour
    {
        private const int kDropdownNum = 5;

        //TODO:最終的には、マスターデータなどから取得したい。
        private static readonly string[][] kEventOptions = new string[][]
        {
            new string[] { "なし" }, 
            new string[] { "増援", "敵が全滅した" },
            new string[] { "増援", "敵が死亡した", "敵A" },
            new string[] { "増援", "敵が死亡した", "敵B" },
            new string[] { "増援", "敵が死亡した", "敵C" },
            new string[] { "宝箱", "バトル開始時" },
            new string[] { "宝箱", "敵が死亡した", "敵A" }, 
            new string[] { "会話", "味方が到達した", "味方A", "会話ID:1" },
            new string[] { "会話", "味方が到達した", "味方B", "会話ID:2" },
            new string[] { "会話", "敵が到達した", "敵A", "会話ID:3" },
            new string[] { "会話", "敵が到達した", "敵B", "会話ID:4" },
            new string[] { "勝利", "味方が到達した" },
            new string[] { "敗北", "敵が到達した" },
        };

        [SerializeField] private Transform m_DropdownList;
        [SerializeField] private Dropdown m_DropdownBase;
        
        private Dropdown[] m_DropdownArr = new Dropdown[kDropdownNum];

        public void Awake()
        {
            for (int cnt = 0; cnt < kDropdownNum; ++cnt)
            {
                m_DropdownArr[cnt] = cnt == 0
                    ? m_DropdownBase
                    : GameObject.Instantiate<Dropdown>(m_DropdownBase, m_DropdownList);
                m_DropdownArr[cnt].name = "Dropdown" + cnt;
                addOnValueChanged(cnt);
                m_DropdownArr[cnt].gameObject.SetActive(cnt == 0);
            }

            //TODO:もっと綺麗なコーディングはないものか。。。
            var arr = kEventOptions.Select(x => x[0]).Distinct().ToArray();
            m_DropdownArr[0].ClearOptions();
            m_DropdownArr[0].AddOptions(arr.ToList());
        }

        private void addOnValueChanged(int idx)
        {
            m_DropdownArr[idx].onValueChanged.AddListener((int value) => OnChangedDropdown(idx, value));            
        }
        
        public void OnChangedDropdown(int idx, int value)
        {
            reflectDropdownOptions(idx, value);
        }

        private void reflectDropdownOptions(int idx, int value)
        {
            //TODO:最終的に作りを考えた方がいい。
            //******* ここから *******
            int next = idx + 1;
            int next2 = next + 1;
            if (next < m_DropdownArr.Length)
            {
                m_DropdownArr[next].ClearOptions();
                string selected = m_DropdownArr[idx].options[value].text;
                var idxArr = kEventOptions.Where(x => idx < x.Length && x[idx] == selected).ToArray();
                var nextList = idxArr.Where(x => next < x.Length).Select(x => x[next]).Distinct().ToList();
                m_DropdownArr[next].AddOptions(nextList);
                m_DropdownArr[next].gameObject.SetActive(0 < nextList.Count);
                
                if (next2 < m_DropdownArr.Length)
                {
                    if (next2 < idxArr[0].Length)
                    {
                        m_DropdownArr[next2].ClearOptions();
                        string first = m_DropdownArr[next].options[0].text;
                        var arr = kEventOptions.Where(x => next < x.Length && x[next] == first).ToArray();
                        var list = arr.Where(x => next2 < x.Length).Select(x => x[next2]).Distinct().ToList();
                        m_DropdownArr[next2].AddOptions(list);
                        m_DropdownArr[next2].gameObject.SetActive(0 < list.Count);
                    }
                    else
                    {
                        m_DropdownArr[next2].gameObject.SetActive(false);
                    }
                }
            }

            for (int i = next2 + 1; i < m_DropdownArr.Length; ++i)
            {
                m_DropdownArr[i].gameObject.SetActive(false);
            }
            //******* ここまで *******
        }

        private void addDropdownOptions(int idx)
        {
            
        }
    }
}