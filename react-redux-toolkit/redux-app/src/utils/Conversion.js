export const convertDateFormat = (date) => {
  let convertedDate = new Date(date);
  return convertedDate.toDateString();
};

export const ConvertDateISOString = (date) => {
  return new Date(date).toISOString().slice(0, 10);
};
