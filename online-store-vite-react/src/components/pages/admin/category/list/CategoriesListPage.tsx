import { useEffect, useState } from "react";
import http_common from "../../../../../http_common.ts";
import ModalDelete from "../../../../../common/admin/ModalDelete.tsx";
import { useNavigate } from "react-router-dom";
import SearchGroup from "../../../../../common/admin/SearchGroup.tsx";
import PaginationGroup from "../../../../../common/admin/PaginationGroup.tsx";
import { IFilter, IPaginatedList } from "../../../../../entities/Filter.ts";
import { ICategory } from "../../../../../entities/Category.ts";
import CreateButton from "../../../../../common/admin/CreateButtonGroup.tsx";
import useSort from "../../../../../hooks/useSort.ts";
import useSearch from "../../../../../hooks/useSearch.ts";
import usePage from "../../../../../hooks/usePage.ts";
import SelectGroup from "../../../../../common/admin/SelectGroup.tsx";
import { Form, Formik } from "formik";
import { toast } from "react-toastify";
import useFilterQueryString from "../../../../../hooks/useFilterQueryString.ts";
import SortableHeader from "../../../../../common/admin/SortableHeader.tsx";
import { BsFillTrashFill } from "react-icons/bs";
import { FaEdit } from "react-icons/fa";
import parse from "html-react-parser";

function CategoriesList() {
  const sortOptions = [
    { id: 0, name: "Id", label: "Id" },
    { id: 1, name: "Name", label: "Name" },
    { id: 2, name: "Description", label: "Description" },
    { id: 3, name: "ParentCategoryId", label: "Parent category id" },
  ];

  const sortDirections = [
    { id: 0, name: "asc", label: "Ascending" },
    { id: 1, name: "desc", label: "Descending" },
  ];

  const [CategoriesPaginatedList, setCategoriesPaginatedList] = useState<
    IPaginatedList<ICategory>
  >({ totalPages: 0, items: [] });

  const navigate = useNavigate();

  const { convertFilterToQueryString } = useFilterQueryString();

  useEffect(() => {
    fetchCategories(initialFilterValues);
  }, []);

  const queryParams = new URLSearchParams(location.search);

  const [initialFilterValues, setInitialFilterValues] = useState<IFilter>({
    pageNumber: parseInt(queryParams.get("page") ?? "1"),
    pageSize: 10,
    searchTerm: queryParams.get("search") ?? "",
    sortBy: queryParams.get("sort") ?? "Id",
    sortDirection: queryParams.get("direction") ?? "asc",
    sorting: queryParams.get("sorting") ?? "",
  });

  const fetchCategories = async (values: IFilter) => {
    await http_common
      .get<IPaginatedList<ICategory>>(
        `api/Categories/getByFilter?${convertFilterToQueryString(values)}`,
      )
      .then((resp) => {
        setCategoriesPaginatedList(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to fetch categories data");
        console.error("Error fetching categories data:", error);
      });
  };

  const { handleSortChange } = useSort({
    setInitialFilterValues,
    fetchData: fetchCategories,
  });

  const handleDelete = async (id: number | string) => {
    await http_common
      .delete(`api/Categories/${id}`)
      .then(() => {
        toast("Category deleted successfully");
        fetchCategories(initialFilterValues);
      })
      .catch((error) => {
        toast("Failed to delete category");
        console.error("Error deleting category:", error);
      });
  };

  const { handleSearchChange } = useSearch({
    setInitialFilterValues,
    fetchData: fetchCategories,
  });

  const { handlePageChange } = usePage({
    setInitialFilterValues,
    fetchData: fetchCategories,
  });

  const findSortById = () => {
    const selectedSortByName = initialFilterValues.sortBy;
    const foundSortOption = sortOptions.find(
      (option) => option.name === selectedSortByName,
    );
    return foundSortOption ? foundSortOption.id : null;
  };

  return (
    <>
      <div className="p-5 sm:ml-64">
        <div>
          <Formik
            initialValues={initialFilterValues}
            onSubmit={fetchCategories}
            enableReinitialize={true}
          >
            {({ handleChange, errors, touched, handleBlur }) => (
              <Form className="flex gap-5">
                <SearchGroup
                  onSearch={handleSearchChange}
                  label="Search Categories"
                  value={initialFilterValues.searchTerm}
                ></SearchGroup>
                <CreateButton label={"Add Category"}></CreateButton>
                <SelectGroup
                  label="Sort by"
                  field="sortBy"
                  onChange={(selectedItem) => {
                    const selectedSortOption = sortOptions.find(
                      (option) => option.id === selectedItem,
                    );
                    if (selectedSortOption) {
                      handleSortChange({
                        ...initialFilterValues,
                        sortBy: selectedSortOption.name,
                      });
                    }
                  }}
                  handleChange={handleChange}
                  error={errors.sortBy}
                  touched={touched.sortBy}
                  handleBlur={handleBlur}
                  options={sortOptions}
                  optionKey="id"
                  optionLabel="name"
                  initialItemId={findSortById()}
                ></SelectGroup>
                <SelectGroup
                  label="Sort direction"
                  field="sortDirection"
                  onChange={(selectedItem) => {
                    const selectedDirection = sortDirections.find(
                      (dir) => dir.id === selectedItem,
                    );
                    if (selectedDirection) {
                      handleSortChange({
                        ...initialFilterValues,
                        sortDirection: selectedDirection.name,
                      });
                    }
                  }}
                  handleChange={handleChange}
                  error={errors.sortDirection}
                  touched={touched.sortDirection}
                  handleBlur={handleBlur}
                  options={sortDirections}
                  optionKey="id"
                  optionLabel="name"
                  initialItemId={
                    sortDirections.find(
                      (dir) => dir.name === initialFilterValues.sortDirection,
                    )?.id ?? null
                  }
                ></SelectGroup>
              </Form>
            )}
          </Formik>
        </div>
        <div className="flex-grow overflow-hidden rounded-lg border border-gray-200 shadow-md mt-5">
          <div className="overflow-x-auto">
            <table className="w-full border-collapse bg-white text-left text-sm text-gray-500">
              <thead className="bg-gray-50">
                <tr>
                  <SortableHeader
                    sortOptions={sortOptions}
                    initialFilterValues={initialFilterValues}
                    handleSortChange={handleSortChange}
                  />
                  <th className="px-4 sm:px-6 py-3 sm:py-4 font-medium text-gray-900 w-1/5">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 border-t border-gray-100">
                {CategoriesPaginatedList.items.map((category) => (
                  <tr key={category.id} className="hover:bg-gray-50">
                    <th className="px-6 py-4">{category.id}</th>
                    <td className="flex gap-3 px-6 py-4 font-normal text-gray-900">
                      <div className="relative h-10 w-10">
                        <img
                          className="h-10 w-10 rounded-full object-contain object-center max-w-none mr-5"
                          src={`${http_common.getUri()}/images/300_${
                            category.image
                          }`}
                          alt=""
                        />
                      </div>
                      <div className="text-sm">
                        <div className="text-gray-400">{category.name}</div>
                      </div>
                    </td>
                    <td className="px-6 py-4">{parse(category.description)}</td>
                    <td className="px-6 py-4">{category.parentCategoryId}</td>
                    <td className="px-6 py-4">
                      <div className="flex justify-end gap-4">
                        <ModalDelete
                          id={category.id}
                          text={category.name}
                          deleteFunc={handleDelete}
                        >
                          <button type="button">
                            <BsFillTrashFill size={20} />
                          </button>
                        </ModalDelete>
                        <button
                          type="button"
                          onClick={() => navigate(`update/${category.id}`)}
                        >
                          <FaEdit size={20} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
        <PaginationGroup
          totalPages={CategoriesPaginatedList.totalPages}
          onPageChange={handlePageChange}
          currentPage={initialFilterValues.pageNumber}
        ></PaginationGroup>
      </div>
    </>
  );
}

export default CategoriesList;
