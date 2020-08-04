using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class Distribution {
        /// <summary>
        /// 从小到大排的，distribution
        /// </summary>
        public List<KeyValuePair<Card, float>> cardPDFs = new List<KeyValuePair<Card, float>>();

        public float maxPDF {
            get {
                if (cardPDFs != null && cardPDFs.Count > 0) {
                    return cardPDFs[cardPDFs.Count - 1].Value;
                }
                return 0f;
            }
        }

        // 根据给定的pdf值，返回对应item的card
        // 超出范围的话，取上界和下界
        public Card Sample(float pdfValue) {
            int i = 0;
            while (i < cardPDFs.Count && cardPDFs[i].Value <= pdfValue) {
                i++;
            }
            // i 此时是第一个超出边界的下标， 所以应该取i-1（除非i == 0， 即取第一个）
            return cardPDFs[Mathf.Max(0, i - 1)].Key;
        }

        // 会删除item，并修改pdf(不会归一化)
        public void EraseFromDistribution(Card target) {
            int index = cardPDFs.Count;
            for (int i = 0; i < cardPDFs.Count; i++) {
                if (cardPDFs[i].Key == target) {
                    index = i;
                    break;
                }
            }

            // 必须存在
            if (index < cardPDFs.Count) {
                // 计算被删除的cdf
                float targetCDF = cardPDFs[index].Value - index - 1 >= 0 ? cardPDFs[index - 1].Value : 0;
                for (int i = index; i < cardPDFs.Count - 1; i++) {
                    var k = cardPDFs[i + 1].Key;
                    var pdf = cardPDFs[i + 1].Value - targetCDF;
                    cardPDFs[i] = new KeyValuePair<Card, float>(k, pdf);
                }
                // 删除末尾的无效元素
                cardPDFs.RemoveAt(cardPDFs.Count - 1);
            }
        }
    }
}