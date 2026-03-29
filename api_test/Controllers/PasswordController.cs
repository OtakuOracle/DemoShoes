using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        //Метод 2
        [HttpPost("validate")]
        public IActionResult ValidatePassword([FromBody] PasswordRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Password))
                    return BadRequest("Поле password обязательно для заполнения");

                string pass = request.Password;

                bool hasMinLength = pass.Length >= 8;
                bool hasDigit = Regex.IsMatch(pass, @"\d");
                bool hasSpecialSymbols = Regex.IsMatch(pass, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]");
                bool hasUpperCase = Regex.IsMatch(pass, @"[A-Z]");

                int rulesCount = 0;
                if (hasMinLength) rulesCount++;
                if (hasDigit) rulesCount++;
                if (hasSpecialSymbols) rulesCount++;
                if (hasUpperCase) rulesCount++;

                string strength;
                bool isValid;

                if (rulesCount <= 1)
                {
                    strength = "Weak";
                    isValid = false;
                }
                else if (rulesCount == 2 || rulesCount == 3)
                {
                    strength = "Medium";
                    isValid = false;
                }
                else
                {
                    strength = "Strong";
                    isValid = true;
                }

                var remarks = new List<string>();
                if (!hasMinLength) remarks.Add("Пароль должен содержать минимум 8 символов");
                if (!hasDigit) remarks.Add("Пароль должен содержать хотя бы одну цифру");
                if (!hasSpecialSymbols) remarks.Add("Пароль должен содержать хотя бы один специальный символ");
                if (!hasUpperCase) remarks.Add("Пароль должен содержать хотя бы одну заглавную букву");

                return Ok(new
                {
                    isValid,
                    strength,
                    remarks
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        public class PasswordRequest
        {
            public string Password { get; set; }
        }

        //Метод 1
        [HttpGet("generate")]

        public async Task<IActionResult> GeneratePass(int lenght = 12, bool includeNumbers = true, bool includeSymbols = false)
        {
            try
            {
                if (lenght < 6 || lenght > 32)
                {
                    return BadRequest("Длина пароля неверная");
                }

                const string letters = "qwertyyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
                const string numbers = "0123456789";
                const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";


                var charPool = letters;
                if (includeNumbers) charPool += numbers;
                if (includeSymbols) charPool += symbols;

                var random = new Random();
                var passwordChar = new List<char>();


                if (includeNumbers)
                {
                    passwordChar.Add(numbers[random.Next(numbers.Length)]);

                }

                if (includeSymbols)
                {
                    passwordChar.Add(symbols[random.Next(symbols.Length)]);
                }

                for (int i = passwordChar.Count; i < lenght; i++)
                {
                    passwordChar.Add(charPool[random.Next(charPool.Length)]);
                }


                string password = new string(passwordChar.ToArray());

                return Ok(new
                {
                    password,
                    lenght,
                    includeNumbers,
                    includeSymbols,
                    generatedAt = DateTime.UtcNow.ToString("o")
                });


            }

            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }

        }




        }
    }