using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticalMade.NumbersToWords
{
    public static class DollarAmountExtensions
    {
        /// <summary>
        /// Converts the given decimal to an English expression
        /// of dollars and cents in string format
        /// </summary>
        public static string ToDollarsAndCents(this decimal number)
        {
            var response = new StringBuilder();

            // Add whole-dollar values in
            var dollars = (long)Math.Truncate(number);
            response.Append(dollars.ToEnglish())
                .AppendSpace()
                .Append(dollars == 1 ? Words.Dollar : Words.Dollars);

            // Now, calculate cents

            // Negatives mess with the modulo math, and
            // are already handled by the whole-dollar
            // English expression
            var absoluteNumber = Math.Abs(number);
            // Total cents in the entire number
            var totalCents = Math.Truncate(absoluteNumber * 100);
            // Just the cent value from the number
            var justCents = totalCents % 100;
            // Cast back to an integral type
            var cents = (long)justCents;

            // Only add the cent value if it's non-zero
            if (cents > 0)
            {
                response.AppendSpace()
                .Append(Words.And)
                .AppendSpace()
                .Append(cents.ToEnglish())
                .AppendSpace()
                .Append(cents == 1 ? Words.Cent : Words.Cents);
            }

            return response.ToString();
        }

        /// <summary>
        /// Converts an integral value to an English
        /// expression housed in a StringBuilder
        /// </summary>
        /// <remarks>
        /// This method began life as a cribbed StackOverflow
        /// answer, but changed shape completely as it started
        /// having to handle more and more cases.
        ///
        /// There does not appear to be grammatical concensus
        /// regarding the English used to join groups of numbers
        /// together (no comma, comma, "and"). This implementation's
        /// handling of those cases is based on a smattering of web
        /// resources and personal feel. See
        /// DollarAmountExtensionsTests.cs for examples of how
        /// particular cases are rendered.
        /// </remarks>
        public static StringBuilder ToEnglish(this long number)
        {
            // One edge case to handle
            if (number == 0)
            {
                return new StringBuilder(Words.Zero);
            }

            var response = new StringBuilder();

            // Handle negatives
            if (number < 0)
            {
                number = -number;
                response.Append(Words.Negative)
                    .AppendSpace();
            }

            // Break the number into its indexed digit groups.
            // See DigitGroup for its definition.
            var digitGroups = number
                // Split the number into digit groups.
                // These come back smallest to largest
                .SplitByDigitGroups()
                // Add an index to the groupings
                .WithIndex()
                // Order from largest to smallest for english-ing
                .Reverse()
                // Encapsulate in a grouping object
                .Select(a => new DigitGroup
                {
                    Index = a.Index,
                    GroupValue = a.Value
                })
                // We only need to deal with groups
                // that have non-zero values
                .Where(a => a.GroupValue > 0);

            // We now have our number chunked into digit groupings,
            // each of which can provide its own english text. Now,
            // we have to piece them together and determine the
            // proper join expression to use for each pair of groups
            DigitGroup? previousGroup = null;
            foreach (var group in digitGroups)
            {
                if (previousGroup != null)
                {
                    if (group.Index == 0 && group.GroupValue < 100)
                    {
                        // Special handler for entry into the smallest
                        // group with an under-one-hundred value. For
                        // example, this renders "one million and five"
                        // rather than "one million, five" for easier
                        // reading.
                        response.AppendSpace()
                            .Append(Words.And)
                            .AppendSpace();
                    }
                    else
                    {
                        // Default comma separation
                        response.Append(Words.Comma)
                            .AppendSpace();
                    }
                }

                // Add the group's text to the response
                response.Append(group.EnglishValue);
                previousGroup = group;
            }

            return response;
        }

        /// <summary>
        /// Converts a number to a stream of digit groups, starting
        /// with the smallest and ending with the largest. Digit groups
        /// are defined as the chunks of up to three numbers created
        /// by adding commas to a number for readability.
        /// </summary>
        private static IEnumerable<int> SplitByDigitGroups(this long number)
        {
            if (number < 0)
            {
                number = Math.Abs(number);
            }

            for (var i = number; i > 0; i /= 1000)
            {
                yield return (int)(i % 1000);
            }
        }

        /// <summary>
        /// An abstraction of the grouping of digits formed by
        /// formatting a large number with commas.
        /// </summary>
        public class DigitGroup
        {
            /// <summary>
            /// Index from smallest digit grouping place to
            /// largest
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// Numeric value of the 1-3 digits forming this
            /// group
            /// </summary>
            public int GroupValue { get; set; }

            /// <summary>
            /// English representation of the group's numeric
            /// value, including its suffix, if applicable
            /// </summary>
            public StringBuilder EnglishValue
            {
                get
                {
                    // All entries to this StringBuilder are expected
                    // to add a space after the added string. The final
                    // space will be truncated before returning.
                    var response = new StringBuilder();

                    // First, hundreds
                    var hundreds = GroupValue / 100;
                    if (hundreds > 0)
                    {
                        response.Append(Words.OnesValues[hundreds])
                            .AppendSpace()
                            .Append(Words.Hundred)
                            .AppendSpace();
                    }

                    // Next, tens and ones. Thanks, evolved language, for
                    // making two different mathematical places a linguistic
                    // continuum.
                    var tensAndOnes = GroupValue % 100;

                    if (hundreds != 0 && tensAndOnes != 0
                        && Index == 0)
                    {
                        // Add "and" between the hundreds and
                        // tens/ones if we're in the smallest group
                        response.Append(Words.And)
                            .AppendSpace();
                    }

                    if (tensAndOnes >= 20)
                    {
                        var tens = tensAndOnes / 10;
                        // Tens and ones work as an etymological chunk,
                        // space is added to the end after ones are handled
                        response.Append(Words.TensValues[tens]);

                        var ones = tensAndOnes % 10;
                        if (ones > 0)
                        {
                            // Values between 20 and 99 receive dashes
                            // between the tens and ones words
                            response.Append(Words.Dash)
                                .Append(Words.OnesValues[ones]);
                        }

                        response.AppendSpace();
                    }
                    else if (tensAndOnes > 0)
                    {
                        // This handles anything between 1 and 19
                        response.Append(Words.OnesValues[tensAndOnes])
                            .AppendSpace();
                    }

                    // Finally, add the digit group suffix, if applicable
                    if (Index != 0 && GroupValue != 0)
                    {
                        response.Append(Words.DigitGroupSuffixes[Index])
                            .AppendSpace();
                    }

                    // Remove the trailing space
                    // https://stackoverflow.com/a/17215160/570190
                    response.Length--;
                    return response;
                }
            }
        }

        private static StringBuilder AppendSpace(this StringBuilder sb) => sb.Append(' ');

        private static class Words
        {
            public const string And = "and";
            public const string Cent = "cent";
            public const string Cents = "cents";
            public const char Comma = ',';
            public const char Dash = '-';
            public const string Dollar = "dollar";
            public const string Dollars = "dollars";
            public const string Hundred = "hundred";
            public const string Negative = "negative";
            public const string Zero = "zero";

            // Handles all ones values through 19
            public static readonly string[] OnesValues = new[]
            {
                string.Empty, // Placeholder to consume the 0 index
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
            };

            public static readonly string[] TensValues = new[]
            {
                string.Empty, // Placeholder to consume the 0 index
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
            };

            /// <summary>
            /// Indexed by digit group from smallest to largest
            /// </summary>
            public static readonly string[] DigitGroupSuffixes = new[]
            {
                string.Empty, // placeholder to consume the 0 index
                "thousand",
                "million",
                "billion",
                "trillion",
                "quadrillion",
                "quintillion"
                // Please don't generate lien waivers for more money
                // than exists
            };
        }
    }
}
