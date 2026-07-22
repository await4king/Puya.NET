using Puya.Base;
using Puya.Collections;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Puya.Service
{
    public class ValidationRequest<TAttribute>
    {
        public PropertyInfo Prop { get; set; }
        public object Value { get; set; }
        public ServiceResponse Response { get; set; }
        public object Request { get; set; }
        public TAttribute Attribute { get; set; }
    }
    public class ValidationItemRequest<TAttribute>
    {
        public PropertyInfo Prop { get; set; }
        public string Value { get; set; }
        public TAttribute Attribute { get; set; }
        public object Bag { get; set; }
    }
    public class ServiceRequestValidator : IServiceRequestValidator
    {
        #region Helpers
        protected bool TryGetCustomAttribute<TAttribute>(PropertyInfo prop, out TAttribute attribute) where TAttribute : ValidationAttribute
        {
            attribute = prop.GetCustomAttribute<TAttribute>();

            return attribute != null;
        }
        bool GetDecimal(object obj, out decimal value)
        {
            var result = false;
            value = default;

            if (obj != null)
            {
                if (obj.GetType().IsBasicType())
                {
                    try
                    {
                        value = (decimal)Convert.ChangeType(obj, TypeHelper.TypeOfDecimal);

                        result = true;
                    }
                    catch (Exception)
                    { }
                }
                else if (obj.GetType() == TypeHelper.TypeOfString)
                {
                    var str = obj.ToString();

                    if (!string.IsNullOrEmpty(str))
                    {
                        result = decimal.TryParse(str, out value);
                    }
                }
            }

            return result;
        }
        protected void ReportError(PropertyInfo prop, ServiceResponse res, string status, object bag = null)
        {
            var sr = new ServiceResponse();

            sr.SetStatus(status);
            sr.Info = prop.Name;
            sr.Bag = bag;
            sr.MessageKey = "Validation";
            sr.MessageArgs.Add("prop", prop.Name);

            if (bag != null)
            {
                var dynamicBag = bag as IDictionary<string, object>;

                if (dynamicBag != null)
                {
                    foreach (var key in dynamicBag.Keys)
                    {
                        sr.MessageArgs.Add(key, dynamicBag[key]);
                    }
                }
                else
                {
                    ReflectionHelper.ForEachPublicInstanceReadableProperty(bag.GetType(), p => sr.MessageArgs.Add(p.Name, p.GetValue(bag)));
                }
            }

            res.InnerResponses.Add(sr);
        }
        protected bool Validate<TAttribute>(PropertyInfo prop,
                                object req,
                                ServiceResponse res,
                                Func<ValidationRequest<TAttribute>, ServiceResponse> fnValidate)
             where TAttribute : ValidationAttribute
        {
            var isValid = true;

            if (prop != null && req != null && res != null && fnValidate != null && TryGetCustomAttribute(prop, out TAttribute attr))
            {
                var reqAttr = attr as RequiredAttribute;
                var jsontypeAttr = attr as DataTypeAttribute;
                var value = prop.GetValue(req);
                var vreq = new ValidationRequest<TAttribute> { Prop = prop, Value = value, Response = res, Request = req, Attribute = attr };
                var errorStatus = string.Empty;
                var errorBag = null as object;
                var errorException = null as Exception;
                var extraBag = new { Rule = typeof(TAttribute).Name.Replace("Attribute", "") };

                if (reqAttr != null)
                {
                    isValid = value != null;

                    if (isValid && vreq.Value is string strValue)
                    {
                        if (reqAttr.IncludeEmptyStrings)
                        {
                            isValid = !string.IsNullOrEmpty(strValue);
                        }
                        if (isValid && reqAttr.IncludeWhiteStrings)
                        {
                            isValid = !string.IsNullOrWhiteSpace(strValue);
                        }
                    }

                    if (!isValid)
                    {
                        errorStatus = "Required";
                    }
                }
                else if (!string.IsNullOrEmpty(value?.ToString()) || attr.RequiresNullCheck)
                {
                    if (jsontypeAttr != null && !jsontypeAttr.Type.IsValidJsonType(value))
                    {
                        errorStatus = "TypeMismatch";
                        errorBag = new { Expected = jsontypeAttr.Type.ToString() };
                        isValid = false;
                    }
                    else
                    {
                        try
                        {
                            var vres = fnValidate(vreq);

                            isValid = vres.Success;
                            errorBag = vres.Bag;
                            errorStatus = vres.Status;
                            errorException = vres.Exception;

                            if (!isValid && string.IsNullOrEmpty(errorStatus))
                            {
                                errorStatus = "Invalid";
                                errorBag = vres.Bag.Merge(extraBag);
                            }
                        }
                        catch (Exception e)
                        {
                            errorStatus = "ValidationFailed";
                            errorException = e;
                            errorBag = extraBag;
                        }
                    }
                }

                if (!isValid)
                {
                    if (errorStatus.StartsWith(":"))
                    {
                        errorStatus = prop.Name + errorStatus.Substring(1);
                    }

                    ReportError(prop, res, errorStatus, errorBag);

                    if (errorException != null)
                    {
                        res.InnerResponses[res.InnerResponses.Count - 1].Exception = errorException;
                    }
                }
            }

            return isValid;
        }
        protected bool ValidateList<TAttribute>(PropertyInfo prop, object req, ServiceResponse res, Func<ValidationItemRequest<TAttribute>, bool> fnValidate, string name = "")
            where TAttribute : ListAttribute
        {
            return Validate<TAttribute>(prop, req, res, (vr) =>
            {
                var attr = vr.Attribute;
                var value = vr.Value as string;

                if (string.IsNullOrEmpty(value))
                {
                    return ServiceResponse.FromStatus("NoItems");
                }

                var attrName = string.IsNullOrEmpty(name) ? typeof(TAttribute).Name.Replace("Attribute", ""): name;

                if (attrName.EndsWith("s"))
                {
                    attrName = attrName.Substring(0, attrName.Length - 1);
                }

                var items = value.Split(new string[] { attr.Separator }, StringSplitOptions.None);
                var isValid = items.Length >= attr.MinCount && (attr.MaxCount == -1 || items.Length <= attr.MaxCount);
                var invalidItem = "";

                if (isValid)
                {
                    var vir = new ValidationItemRequest<TAttribute> { Attribute = attr, Prop = prop };
                    var i = 0;

                    foreach (var item in items)
                    {
                        vir.Value = item?.Trim();

                        var itemIsValid = !string.IsNullOrEmpty(attr.Pattern)
                            ? System.Text.RegularExpressions.Regex.IsMatch(item.Trim(), attr.Pattern)
                            : fnValidate(vir);

                        if (!itemIsValid)
                        {
                            invalidItem = item;
                            isValid = false;
                            break;
                        }

                        i++;
                    }

                    return ServiceResponse.FromStatus(isValid ? "Success" : "Invalid" + attrName)
                                          .SetBag(new { Item = invalidItem, Index = i }.Merge(vir.Bag));
                }
                else
                {
                    return ServiceResponse.FromStatus(isValid ? "Success" : "ItemCountMismatch")
                                          .SetBag(new { attr.MinCount, attr.MaxCount });
                }
            });
        }
        protected bool ValidatePattern<TAttribute>(PropertyInfo prop, object req, ServiceResponse res, Func<ValidationItemRequest<TAttribute>, bool> fnValidate, string name)
            where TAttribute : RegExpAttribute
        {
            return Validate<TAttribute>(prop, req, res, (vr) =>
            {
                var isValid = true;
                var attr = vr.Attribute;
                var value = vr.Value as string;
                var vir = new ValidationItemRequest<TAttribute>
                {
                    Attribute = attr,
                    Value = value,
                    Prop = prop,
                };

                if (!string.IsNullOrEmpty(attr.Pattern))
                {
                    isValid = string.IsNullOrEmpty(value) || System.Text.RegularExpressions.Regex.IsMatch(value, attr.Pattern);
                }
                else if (fnValidate != null)
                {
                    isValid = string.IsNullOrEmpty(value) || fnValidate(vir);
                }

                if (name.StartsWith(":"))
                {
                    return ServiceResponse.FromStatus(isValid ? "Success" : "Invalid" + name.Substr(1))
                                          .SetBag(vir.Bag)
                                          .SetInfo(name.Substring(1));
                }
                else
                {
                    return ServiceResponse.FromStatus(isValid ? "Success" : name);
                }
            });
        }
        #endregion
        #region Rules
        protected bool CheckRequiredRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<RequiredAttribute>(prop, req, res, (vr) => ServiceResponse.FromSucceeded());
        }
        protected bool CheckMinValueRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<MinValueAttribute>(prop, req, res, (vr) =>
            {
                var response = new ServiceResponse();

                if (GetDecimal(vr.Value, out decimal numericValue))
                {
                    if (numericValue >= vr.Attribute.Value)
                    {
                        response.Succeeded();
                    }
                    else
                    {
                        response.SetStatus("ValueTooSmall");
                        response.Bag = new { Min = vr.Attribute.Value };
                    }
                }
                else
                {
                    response.SetStatus("NotNumeric");
                }

                return response;
            });
        }
        protected bool CheckMaxValueRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<MaxValueAttribute>(prop, req, res, (vr) =>
            {
                var response = new ServiceResponse();

                if (GetDecimal(vr.Value, out decimal numericValue))
                {
                    if (numericValue <= vr.Attribute.Value)
                    {
                        response.Succeeded();
                    }
                    else
                    {
                        response.SetStatus("ValueTooLarge");
                        response.Bag = new { Max = vr.Attribute.Value };
                    }
                }
                else
                {
                    response.SetStatus("NotNumeric");
                }

                return response;
            });
        }
        protected bool CheckLenRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<LenAttribute>(prop, req, res, (vr) =>
            {
                var response = new ServiceResponse();
                var value = vr.Value as string;

                if (value?.Length == vr.Attribute.Value)
                {
                    response.Succeeded();
                }
                else
                {
                    response.SetStatus("IncorrectLength");
                    response.Bag = new { ExpectedLength = vr.Attribute.Value, CurrentLength = value?.Length ?? 0 };
                }

                return response;
            });
        }
        protected bool CheckMinLenRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<MinLenAttribute>(prop, req, res, (vr) =>
            {
                var response = new ServiceResponse();
                var value = vr.Value as string;

                if (value?.Length >= vr.Attribute.MinLen)
                {
                    response.Succeeded();
                }
                else
                {
                    response.SetStatus("LengthTooSmall");
                    response.Bag = new { MinLength = vr.Attribute.MinLen, CurrentLength = value?.Length ?? 0 };
                }

                return response;
            });
        }
        protected bool CheckMaxLenRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<MaxLenAttribute>(prop, req, res, (vr) =>
            {
                var response = new ServiceResponse();
                var value = vr.Value as string;

                if (value.Length <= vr.Attribute.MaxLen)
                {
                    response.Succeeded();
                }
                else
                {
                    response.SetStatus("LengthTooLarge");
                    response.Bag = new { MaxLength = vr.Attribute.MaxLen, CurrentLength = value.Length };
                }

                return response;
            });
        }
        protected bool CheckAlphaRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<AlphaAttribute>(prop, req, res, (vr) =>
                ServiceResponse.FromStatus((vr.Value as string).All(char.IsLetter) ? "Success" : "NotAlpha")
            );
        }
        protected bool CheckAlphaNumRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<AlphaNumAttribute>(prop, req, res, (vr) =>
                ServiceResponse.FromStatus((vr.Value as string).All(char.IsLetterOrDigit) ? "Success" : "NotAlphaNum")
            );
        }
        protected bool CheckNumericRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<NumericAttribute>(prop, req, res, (vr) =>
            {
                if (GetDecimal(vr.Value, out decimal d))
                {
                    return ServiceResponse.FromStatus("Success");
                }

                return ServiceResponse.FromStatus("NotNumeric");
            });
        }
        protected bool CheckNumericIntRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<NumericIntAttribute>(prop, req, res, (vr) =>
            {
                if (GetDecimal(vr.Value, out decimal d))
                {
                    if (Math.Floor(d) != d)
                    {
                        return ServiceResponse.FromStatus("NotNumericInt");
                    }
                    
                    return ServiceResponse.FromStatus("Success");
                }

                return ServiceResponse.FromStatus("NotNumeric");
            });
        }
        protected bool CheckNotNegativeRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<NotNegativeAttribute>(prop, req, res, (vr) =>
            {
                if (GetDecimal(vr.Value, out decimal d))
                {
                    if (d < 0)
                    {
                        return ServiceResponse.FromStatus("IsNegative");
                    }

                    return ServiceResponse.FromStatus("Success");
                }

                return ServiceResponse.FromStatus("NotNumeric");
            });
        }
        protected bool CheckNotZeroRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<NotZeroAttribute>(prop, req, res, (vr) =>
            {
                if (GetDecimal(vr.Value, out decimal d))
                {
                    if (d == 0)
                    {
                        return ServiceResponse.FromStatus("IsZero");
                    }

                    return ServiceResponse.FromStatus("Success");
                }

                return ServiceResponse.FromStatus("NotNumeric");
            });
        }
        protected bool CheckRangeRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<RangeAttribute>(prop, req, res, (vr) =>
            {
                var value = vr.Value;
                var str = vr.Value as string;
                var isValid = value.GetType().IsNumeric() || Validation.Validation.IsNumeric(str);
                var attr = vr.Attribute;
                var status = "";
                object bag = null;

                if (isValid)
                {
                    switch (attr.RangeType)
                    {
                        case RangeType.Byte:
                            bag = new { From = attr.FromByte, To = attr.ToByte };

                            if (value is byte b) isValid = b >= attr.FromByte && b <= attr.ToByte;
                            else if (byte.TryParse(str, out byte bVal)) isValid = bVal >= attr.FromByte && bVal <= attr.ToByte;
                            else status = "NotByte";
                            break;
                        case RangeType.SByte:
                            bag = new { From = attr.FromSByte, To = attr.ToSByte };

                            if (value is sbyte sb) isValid = sb >= attr.FromSByte && sb <= attr.ToSByte;
                            else if (sbyte.TryParse(str, out sbyte sbVal)) isValid = sbVal >= attr.FromSByte && sbVal <= attr.ToSByte;
                            else status = "NotSByte";
                            break;
                        case RangeType.Short:
                            bag = new { From = attr.FromShort, To = attr.ToShort };

                            if (value is short s) isValid = s >= attr.FromShort && s <= attr.ToShort;
                            else if (short.TryParse(str, out short sVal)) isValid = sVal >= attr.FromShort && sVal <= attr.ToShort;
                            else status = "NotShort";
                            break;
                        case RangeType.UShort:
                            bag = new { From = attr.FromUShort, To = attr.ToUShort };

                            if (value is ushort us) isValid = us >= attr.FromUShort && us <= attr.ToUShort;
                            else if (ushort.TryParse(str, out ushort usVal)) isValid = usVal >= attr.FromUShort && usVal <= attr.ToUShort;
                            else status = "NotUShort";
                            break;
                        case RangeType.Int:
                            bag = new { From = attr.FromInt, To = attr.ToInt };

                            if (value is int i) isValid = i >= attr.FromInt && i <= attr.ToInt;
                            else if (int.TryParse(str, out int iVal)) isValid = iVal >= attr.FromInt && iVal <= attr.ToInt;
                            else status = "NotInt";
                            break;
                        case RangeType.UInt:
                            bag = new { From = attr.FromUInt, To = attr.ToUInt };

                            if (value is uint ui) isValid = ui >= attr.FromUInt && ui <= attr.ToUInt;
                            else if (uint.TryParse(str, out uint uiVal)) isValid = uiVal >= attr.FromUInt && uiVal <= attr.ToUInt;
                            else status = "NotUInt";
                            break;
                        case RangeType.Long:
                            bag = new { From = attr.FromLong, To = attr.ToLong };

                            if (value is long l) isValid = l >= attr.FromLong && l <= attr.ToLong;
                            else if (long.TryParse(str, out long lVal)) isValid = lVal >= attr.FromLong && lVal <= attr.ToLong;
                            else status = "NotLong";
                            break;
                        case RangeType.ULong:
                            bag = new { From = attr.FromULong, To = attr.ToULong };

                            if (value is ulong ul) isValid = ul >= attr.FromULong && ul <= attr.ToULong;
                            else if (ulong.TryParse(str, out ulong ulVal)) isValid = ulVal >= attr.FromULong && ulVal <= attr.ToULong;
                            else status = "NotULong";
                            break;
                        case RangeType.Float:
                            bag = new { From = attr.FromFloat, To = attr.ToFloat };

                            if (value is float f) isValid = f >= attr.FromFloat && f <= attr.ToFloat;
                            else if (float.TryParse(str, out float fVal)) isValid = fVal >= attr.FromFloat && fVal <= attr.ToFloat;
                            else status = "NotFloat";
                            break;
                        case RangeType.Double:
                            bag = new { From = attr.FromDouble, To = attr.ToDouble };

                            if (value is double d) isValid = d >= attr.FromDouble && d <= attr.ToDouble;
                            else if (double.TryParse(str, out double dVal)) isValid = dVal >= attr.FromDouble && dVal <= attr.ToDouble;
                            else status = "NotDouble";
                            break;
                        case RangeType.Decimal:
                            bag = new { From = attr.FromDec, To = attr.ToDec };

                            if (value is decimal dec) isValid = dec >= attr.FromDec && dec <= attr.ToDec;
                            else if (decimal.TryParse(str, out decimal decVal)) isValid = decVal >= attr.FromDec && decVal <= attr.ToDec;
                            else status = "NotDecimal";
                            break;
                    }
                }
                else
                {
                    status = "NotNumeric";
                }

                return ServiceResponse.FromStatus(isValid ? "Success" : string.IsNullOrEmpty(status) ? "RangeViolation": status).SetBag(bag);
            });
        }
        protected bool CheckEmailRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<EmailAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsEmail(vi.Value), ":Email");
        }
        protected bool CheckEmailsRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<EmailsAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsEmail(vi.Value));
        }
        protected bool CheckMobileRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<MobileAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsMobile(vi.Value), ":Mobile");
        }
        protected bool CheckMobilesRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<MobilesAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsMobile(vi.Value));
        }
        protected bool CheckPhoneRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<PhoneAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsPhone(vi.Value), ":Phone");
        }
        protected bool CheckPhonesRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<PhonesAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsPhone(vi.Value));
        }
        protected bool CheckIrPhoneRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<IrPhoneAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsIrPhone(vi.Value), ":Phone");
        }
        protected bool CheckIrPhonesRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<IrPhonesAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsIrPhone(vi.Value), "Phone");
        }
        protected bool CheckIPv4Rule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<IPv4Attribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsIPv4(vi.Value, vi.Attribute.Mask), ":IPv4");
        }
        protected bool CheckIPv4sRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<IPv4sAttribute>(prop, req, res, vi => string.IsNullOrEmpty(vi.Value?.ToString()) || Validation.Validation.IsIPv4(vi.Value, vi.Attribute.Mask));
        }
        protected bool CheckRegExpRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidatePattern<RegExpAttribute>(prop, req, res, vi =>
            {
                var isValid = System.Text.RegularExpressions.Regex.IsMatch(vi.Value, vi.Attribute.Pattern);

                if (!isValid)
                {
                    vi.Bag = new { vi.Attribute.Pattern };
                }

                return isValid;
            }, "PatternMismatch");
        }
        protected bool CheckRegExpsRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<RegExpsAttribute>(prop, req, res, vi =>
            {
                var isValid = System.Text.RegularExpressions.Regex.IsMatch(vi.Value, vi.Attribute.Pattern);

                if (!isValid)
                {
                    vi.Bag = new { vi.Attribute.Pattern };
                }

                return isValid;
            });
        }
        protected bool CheckNationalCodeRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            var attr = prop.GetCustomAttribute<NationalCodeAttribute>();

            if (attr == null) return true;

            var value = prop.GetValue(req) as string;
            var result = Validation.Validation.IsNationalCode(value);
            var isValid = result == IsNationalCodeResult.Valid || result == IsNationalCodeResult.NoCode;

            if (!isValid)
            {
                ReportError(prop, res, "InvalidNationalCode", new { Reason = result.ToString() });
            }

            return isValid;
        }
        protected bool CheckOneOfRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return Validate<OneOfAttribute>(prop, req, res, (vr) =>
            {
                var value = vr.Value;
                var strValue = value as string;
                var attr = vr.Attribute;
                var items = attr.Items.Split(new string[] { attr.Separator }, StringSplitOptions.None).Select(i => i.Trim());
                var allowedItems = new HashSet<string>(items, attr.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
                var isValid = string.IsNullOrEmpty(strValue) || allowedItems.Contains(strValue);

                return ServiceResponse.FromStatus(isValid ? "Success" : "InvalidItem").SetBag(new { Allowed = attr.Items });
            });
        }
        protected bool CheckManyOfRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            HashSet<string> allowedItems = null;

            return ValidateList<ManyOfAttribute>(prop, req, res, vi =>
            {
                if (allowedItems == null && !string.IsNullOrEmpty(vi.Value?.ToString()))
                {
                    allowedItems = new HashSet<string>(vi.Attribute.Items.Split(new string[] { vi.Attribute.Separator }, StringSplitOptions.None)
                                        .Select(i => i.Trim()),
                                        vi.Attribute.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
                }

                var result = string.IsNullOrEmpty(vi.Value?.ToString()) || allowedItems.Contains(vi.Value);

                if (!result)
                {
                    vi.Bag = new { Allowed = allowedItems.Join(",") };
                }

                return result;
            }, "Item");
        }
        protected bool CheckListRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<ListAttribute>(prop, req, res, vi => true);
        }
        protected bool CheckMinCountRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<MinCountAttribute>(prop, req, res, vi => true);
        }
        protected bool CheckMaxCountRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            return ValidateList<MaxCountAttribute>(prop, req, res, vi => true);
        }
        protected bool CheckPreventCharsRule(PropertyInfo prop, object req, ServiceResponse res)
        {
            var attr = prop.GetCustomAttribute<PreventCharsAttribute>();
            if (attr == null) return true;

            var value = prop.GetValue(req);
            var strValue = prop.GetValue(req) as string;
            var isValid = false;

            if (value != null && string.IsNullOrEmpty(strValue))
            {
                ReportError(prop, res, "TypeMismatch");
            }
            else
            {
                if (string.IsNullOrEmpty(strValue)) return true;

                isValid = !strValue.Any(c => attr.ExcludedCharacters.Contains(c));

                if (!isValid)
                {
                    ReportError(prop, res, "ContainsExcludedChars", new { Excluded = attr.ExcludedCharacters });
                }
            }

            return isValid;
        }
        #endregion
        protected virtual Task<bool> OnCustomValidate<TRequest, TResponse>(PropertyInfo prop, TRequest req, TResponse res)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            return Task.FromResult(true);
        }
        public virtual async Task<bool> Validate<TRequest, TResponse>(TRequest req, TResponse res)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            var result = false;

            if (req == null)
            {
                res.SetStatus("NoRequest");
            }
            else
            {
                var props = ReflectionHelper.GetPublicInstanceReadableProperties(req.GetType());
                var sortedProps = props.OrderBy(p =>
                {
                    var order = 0;

                    if (TryGetCustomAttribute(p, out OrderAttribute attr))
                    {
                        if (string.IsNullOrEmpty(attr.Subject) || attr.Subject.Equalz("Validation"))
                        {
                            order = attr.Order;
                        }
                    }

                    return order;
                });
                var prevInnerResponseCount = res.InnerResponses.Count;
                var fullValidation = req.GetType().GetCustomAttribute<FullValidation>() != null;

                foreach (var prop in sortedProps)
                {
                    if (!CheckRequiredRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMinValueRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMaxValueRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckLenRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMinLenRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMaxLenRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckAlphaRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckAlphaNumRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckNumericRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckNumericIntRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckNotNegativeRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckNotZeroRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckRangeRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckEmailRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckEmailsRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMobileRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMobilesRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckPhoneRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckPhonesRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckIrPhoneRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckIrPhonesRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckIPv4Rule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckIPv4sRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckNationalCodeRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckOneOfRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckManyOfRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMinCountRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckMaxCountRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckRegExpRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckRegExpsRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!CheckPreventCharsRule(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                    if (!await OnCustomValidate(prop, req, res) && !fullValidation)
                    {
                        break;
                    }
                }

                if (res.InnerResponses.Count > prevInnerResponseCount)
                {
                    res.SetStatus("InvalidRequest");
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
