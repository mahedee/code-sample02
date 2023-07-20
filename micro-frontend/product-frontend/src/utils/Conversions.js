export const convertDateFormat = (date) => {
  let convertedDate = new Date(date);
  return convertedDate.toDateString();
};
