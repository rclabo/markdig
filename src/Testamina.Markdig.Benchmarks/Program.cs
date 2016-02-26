﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Textamina.Markdig.Formatters;
using Textamina.Markdig.Parsers;

namespace Testamina.Markdig.Benchmarks
{
    public class Program
    {
        private string text;

        public Program()
        {
            //text = File.ReadAllText("progit.md");
            text = File.ReadAllText("spec.md");
        }

        [Benchmark]
        public void TestMarkdig()
        {
            //var reader = new StreamReader(File.Open("spec.md", FileMode.Open));
            var reader = new StringReader(text);
            var parser = new MarkdownParser(reader);
            var doc = parser.Parse();
            reader.Dispose();
            //var formatter = new HtmlFormatter(new StringWriter());
            //formatter.Write(doc);
        }

        [Benchmark]
        public void TestCommonMark()
        {
            ////var reader = new StreamReader(File.Open("spec.md", FileMode.Open));
            var reader = new StringReader(text);
            //CommonMark.CommonMarkConverter.Parse(reader);
            CommonMark.CommonMarkConverter.Parse(reader);
            //reader.Dispose();
            //CommonMark.CommonMarkConverter.Convert(new StringReader(text), new StringWriter());
        }

        static void Main(string[] args)
        {
            bool markdig = args.Length == 0;
            bool simpleBench = false;

            if (simpleBench)
            {
                var clock = Stopwatch.StartNew();
                var program = new Program();
                for (int i = 0; i < 300; i++)
                {
                    if (markdig)
                    {
                        program.TestMarkdig();
                    }
                    else
                    {
                        program.TestCommonMark();
                    }
                }
                Console.WriteLine((markdig ? "MarkDig" : "CommonMark") +  $" => time: {clock.ElapsedMilliseconds}ms");
                DumpGC();
            }
            else
            {
                BenchmarkRunner.Run<Program>();
            }
        }

        static void DumpGC()
        {
            Console.WriteLine($"gc0: {GC.CollectionCount(0)}");
            Console.WriteLine($"gc1: {GC.CollectionCount(1)}");
            Console.WriteLine($"gc2: {GC.CollectionCount(2)}");
        }
    }
}
