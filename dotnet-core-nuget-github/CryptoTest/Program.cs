// See https://aka.ms/new-console-template for more information

using CryptoEngine;

string plainText = "I am Mahedee";
Console.WriteLine("Plain Text: {0}", plainText);
Console.WriteLine("Hash value: {0}", CryptoGenerator.GenerateSha256Hash(plainText));