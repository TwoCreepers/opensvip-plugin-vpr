using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Utils
{
    /// <summary>
    /// 音高相关的工具类。
    /// </summary>
    public static class VprPitchUtils
    {
        /// <summary>
        /// 计算该音符的前音和尾音音高补偿点。
        /// </summary>
        /// <param name="note">音符</param>
        /// <returns>前音和尾音补偿点位置</returns>
        public static (int, int) NoteStartAndEndPitchCompensationPoints(Plugin.Vpr.Core.Model.Track.Part.Note.Note note)
        {
            var possibleStart = note.Duration / 4;
            if (possibleStart < 1) possibleStart = 1;
            var possibleEnd = note.Duration - possibleStart;
            if (possibleEnd < 1) possibleEnd = 1;
            const int MaxStart = 35;
            const int MaxEnd = 105;
            return (Math.Min(possibleStart, MaxStart) + note.Position, note.Position + note.Duration - Math.Min(possibleEnd, MaxEnd));
        }
        /// <summary>
        /// 计算 VOCALOID 对该位置最终音高的补偿。（未能完全还原）
        /// </summary>
        /// <param name="notes">音符列表</param>
        /// <param name="noteIndex">当前音符索引</param>
        /// <param name="pitchPointsPos">音高控制点位置</param>
        /// <returns>VOCALOID 对该位置最终音高的补偿</returns>
        public static int PitchPointsCompensation(List<Plugin.Vpr.Core.Model.Track.Part.Note.Note> notes, int noteIndex, int pitchPointsPos)
        {
            // 高情商：我们保留了VOCALOID原始的电音味
            // 低情商：我们技术不行算不好音符前后补偿，轻点喷[求饶]
            const int NoImpactDistance = 60 * 7; // 420
            var (start, end) = NoteStartAndEndPitchCompensationPoints(notes[noteIndex]);
            if (pitchPointsPos > start && pitchPointsPos < end - 20)
            {
                return 0;
            }
            var currentNote = notes[noteIndex]; // 当前音符(总是可用)
            var previousNote = noteIndex > 0 ? notes[noteIndex - 1] : null; // 前一个音符
            var latterNote = noteIndex < notes.Count - 1 ? notes[noteIndex + 1] : null; // 后一个音符
            var isPreviousImpact = previousNote != null && currentNote.Position - (previousNote.Position + previousNote.Duration) < NoImpactDistance;
            var isLatterImpact = latterNote != null && latterNote.Position - (currentNote.Position + currentNote.Duration) < NoImpactDistance;
            if (!isLatterImpact && pitchPointsPos > end && currentNote.Duration <= 465)
            {
                return 0;
            }
            else if (!isLatterImpact && currentNote.Duration > 465 && pitchPointsPos > end - 20)
            {
                // 音高控制点位于尾音补偿点之后的偏移
                var pitchPointsEndCompensationOffset = pitchPointsPos - (end - 20);
                return -(pitchPointsEndCompensationOffset * 256 / (currentNote.Position + currentNote.Duration - end));
            }
            else if (!isLatterImpact && currentNote.Duration > 465 && pitchPointsPos > end - 20)
            {
                return 0;
            }
            else if (!isPreviousImpact && pitchPointsPos < (start = Math.Min(currentNote.Duration / 4, 75) + currentNote.Position))
            {
                // 音高控制点位于前音补偿点之前的偏移
                var pitchPointsStartCompensationOffset = pitchPointsPos - currentNote.Position;
                if (pitchPointsPos < currentNote.Position)
                {
                    return pitchPointsStartCompensationOffset * 64 / 45 - 256;
                }
                return pitchPointsStartCompensationOffset * 256 / (start - currentNote.Position) - 256;    // start - currentNote.Position 不可能等于0
            }
            else if (isLatterImpact && pitchPointsPos > end)
            {
                if (currentNote.Number == latterNote.Number)
                {
                    return 0; // 如果音高相同，则不需要补偿
                }
                var latterStartCompensationPoints = NoteStartAndEndPitchCompensationPoints(latterNote).Item1;
                var compensationLength = latterStartCompensationPoints - end;
                var compensationHeight = (latterNote.Number - currentNote.Number) * 512;
                var pitchPointsEndCompensationOffset = pitchPointsPos - end;
                return pitchPointsEndCompensationOffset * compensationHeight / compensationLength;
            }
            else if (isPreviousImpact && pitchPointsPos < start)
            {
                if (currentNote.Number == previousNote.Number)
                {
                    return 0; // 如果音高相同，则不需要补偿
                }
                var previousEndCompensationPoints = NoteStartAndEndPitchCompensationPoints(previousNote).Item2;
                var compensationLength = previousEndCompensationPoints - start;
                var compensationHeight = (currentNote.Number - previousNote.Number) * 512;
                var pitchPointsStartCompensationOffset = start - pitchPointsPos;
                return pitchPointsStartCompensationOffset * compensationHeight / compensationLength;
            }
            return 0;
        }
    }
}
