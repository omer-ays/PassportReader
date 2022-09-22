using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Dependencies
{
    public interface IPassportAnalyzer
    {
        void Analyzer(string base64Image);
    }
}
