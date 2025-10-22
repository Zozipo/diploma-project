import { useNavigate } from "react-router-dom";
import { IFilter } from "../entities/Filter.ts";

interface UseSortProps {
  setInitialFilterValues: React.Dispatch<React.SetStateAction<IFilter>>;
  fetchData: (values: IFilter) => Promise<void>;
}

const useSort = ({ setInitialFilterValues, fetchData }: UseSortProps) => {
  const navigate = useNavigate();

  const handleSortChange = (newFilterValues: IFilter) => {
    const existingParams = new URLSearchParams(window.location.search);

    existingParams.set("sort", encodeURIComponent(newFilterValues.sortBy));
    existingParams.set("direction", newFilterValues.sortDirection);
    const newParams = existingParams.toString();
    navigate(`?${newParams}`);

    setInitialFilterValues(newFilterValues);
    fetchData(newFilterValues);
  };

  return { handleSortChange };
};

export default useSort;
