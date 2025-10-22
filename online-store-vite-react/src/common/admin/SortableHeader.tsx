import React from "react";
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";

interface SortOption {
  id: number | string;
  name: string;
  label: string;
}

interface SortableHeaderProps {
  sortOptions: SortOption[];
  initialFilterValues: any;
  handleSortChange: (values: any) => void;
}

const SortableHeader: React.FC<SortableHeaderProps> = ({
  sortOptions,
  initialFilterValues,
  handleSortChange,
}) => {
  return (
    <>
      {sortOptions.map((option) => (
        <th
          key={option.id}
          className="px-4 sm:px-6 py-3 sm:py-4 font-medium text-gray-900 hover:bg-gray-100 cursor-pointer w-1/5"
          onClick={() => {
            const selectedSortOption = sortOptions.find(
              (o) => o.id === option.id,
            );
            if (selectedSortOption) {
              let newDirection: string = initialFilterValues.sortDirection;
              if (selectedSortOption.name === initialFilterValues.sortBy) {
                newDirection =
                  initialFilterValues.sortDirection === "asc" ? "desc" : "asc";
              }
              handleSortChange({
                ...initialFilterValues,
                sortBy: selectedSortOption.name,
                sortDirection: newDirection,
              });
            }
          }}
        >
          <div className="flex justify-between">
            <span>{option.label}</span>
            {initialFilterValues.sortBy === option.name &&
              initialFilterValues.sortDirection && (
                <span>
                  {initialFilterValues.sortDirection === "asc" ? (
                    <IoIosArrowDown size={20} />
                  ) : (
                    <IoIosArrowUp size={20} />
                  )}
                </span>
              )}
          </div>
        </th>
      ))}
    </>
  );
};

export default SortableHeader;
