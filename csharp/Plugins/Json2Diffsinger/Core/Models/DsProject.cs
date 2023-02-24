﻿using System.Collections.Generic;
using System.Linq;

namespace Json2DiffSinger.Core.Models
{
    /// <summary>
    /// ds 工程文件
    /// </summary>
    public class DsProject
    {
        #region Properties

        /// <summary>
        /// 音符列表
        /// </summary>
        public List<DsNote> NoteList { get; set; } = new List<DsNote>();

        /// <summary>
        /// 音高参数
        /// </summary>
        public DsParamCurve PitchParamCurve { get; set; } = new DsParamCurve();

        /// <summary>
        /// 性别参数
        /// </summary>
        public DsParamCurve GenderParamCurve { get; set; } = new DsParamCurve();

        #endregion

        /// <summary>
        /// 将 ds 工程转换为 ds 参数 model
        /// </summary>
        /// <param name="dsProject"></param>
        /// <returns></returns>
        public static PhonemeParamsModel ToParamModel(DsProject dsProject)
        {
            #region Serialize Notes

            var dsNotes = dsProject.NoteList;
            string inputText = "";
            string phonemeSeq = "";
            string inputNoteSeq = "";
            string inputDurationSeq = "";
            string isSlurSeq = "";
            string phonemeDurSeq = "";
            for (int i = 0; i < dsNotes.Count; i++)
            {
                var curNote = dsNotes[i];
                var dsPhoneme = curNote.DsPhoneme;
                var consonant = dsPhoneme.Consonant;
                var vowel = dsPhoneme.Vowel;
                inputText += curNote.Lyric.Replace("-", "");
                if (consonant.Phoneme != "")
                {
                    phonemeSeq += consonant.Phoneme + " ";
                    phonemeDurSeq += consonant.Duration + " ";
                    isSlurSeq += "0 ";
                    inputDurationSeq += curNote.Duration + " ";
                    //inputNoteSeq += consonant.NoteName + " ";
                    inputNoteSeq += curNote.NoteName + " ";
                }
                phonemeSeq += vowel.Phoneme;
                phonemeDurSeq += vowel.Duration;
                inputNoteSeq += vowel.NoteName;
                inputDurationSeq += curNote.Duration;
                isSlurSeq += curNote.IsSlur ? "1" : "0";
                if (i < dsNotes.Count - 1)
                {
                    if (!curNote.IsSlur)
                    {
                        inputText += " ";
                    }
                    inputNoteSeq += " ";
                    inputDurationSeq += " ";
                    isSlurSeq += " ";
                    phonemeSeq += " ";
                    phonemeDurSeq += " ";
                }
            }

            #endregion

            #region Serialize Pitch Parameter Curve

            var pitchPoints = dsProject.PitchParamCurve.PointList;
            string f0Sequence = null;
            if (pitchPoints != null && pitchPoints.Any())
            {
                f0Sequence = string.Join(" ", pitchPoints.Select(p => $"{p.Value:F1}"));
            }

            #endregion

            #region Serialize Gender Parameter Curve

            var genderPoints = dsProject.GenderParamCurve.PointList;
            string genderSequence = null;
            if (genderPoints != null && genderPoints.Any())
            {
                genderSequence = string.Join(" ", genderPoints.Select(p => $"{p.Value:F1}"));
            }

            #endregion

            return new PhonemeParamsModel
            {
                LyricText = inputText,
                PhonemeSequence = phonemeSeq,
                NoteSequence = inputNoteSeq,
                NoteDurationSequence = inputDurationSeq,
                IsSlurSequence = isSlurSeq,
                PhonemeDurationSequence = phonemeDurSeq,
                F0TimeStepSize = dsProject.PitchParamCurve.StepSize.ToString(),
                F0Sequence = f0Sequence,
                GenderTimeStepSize = dsProject.GenderParamCurve.StepSize.ToString(),
                GenderSequence = genderSequence
            };
        }
    }
}
