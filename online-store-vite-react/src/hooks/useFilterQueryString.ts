import { IFilter } from "../entities/Filter.ts";

const useFilterQueryString = () => {
  const convertFilterToQueryString = (filter: IFilter): string => {
    const queryParams = Object.entries(filter)
      .filter(
        ([_, value]) => value !== null && value !== undefined && value !== "",
      )
      .map(
        ([key, value]) =>
          `${encodeURIComponent(
            key.charAt(0).toUpperCase() + key.slice(1),
          )}=${encodeURIComponent(value)}`,
      )
      .join("&");
    return queryParams;
  };

  return { convertFilterToQueryString };
};

export default useFilterQueryString;
