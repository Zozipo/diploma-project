import { SetStateAction, useEffect, useState } from "react";
import { IUser } from "../../../../../entities/User.ts";
import http_common from "../../../../../http_common.ts";
import ModalDelete from "../../../../../common/admin/ModalDelete.tsx";
import { useNavigate } from "react-router-dom";
import SearchGroup from "../../../../../common/admin/SearchGroup.tsx";
import PaginationGroup from "../../../../../common/admin/PaginationGroup.tsx";
import { IFilter, IPaginatedList } from "../../../../../entities/Filter.ts";
import useSort from "../../../../../hooks/useSort.ts";
import useSearch from "../../../../../hooks/useSearch.ts";
import usePage from "../../../../../hooks/usePage.ts";
import { Form, Formik } from "formik";
import SelectGroup from "../../../../../common/admin/SelectGroup.tsx";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import useFilterQueryString from "../../../../../hooks/useFilterQueryString.ts";
import SortableHeader from "../../../../../common/admin/SortableHeader.tsx";
import { BsFillTrashFill } from "react-icons/bs";
import { FaEdit } from "react-icons/fa";

function UsersListPage() {
  const [usersPaginatedList, setUsersPaginatedList] = useState<
    IPaginatedList<IUser>
  >({ totalPages: 0, items: [] });

  const navigate = useNavigate();

  const { convertFilterToQueryString } = useFilterQueryString();

  const queryParams = new URLSearchParams(location.search);

  const [initialFilterValues, setInitialFilterValues] = useState<IFilter>({
    pageNumber: parseInt(queryParams.get("page") ?? "1"),
    pageSize: 10,
    searchTerm: queryParams.get("search") ?? "",
    sortBy: queryParams.get("sort") ?? "Name",
    sortDirection: queryParams.get("direction") ?? "asc",
    sorting: queryParams.get("sorting") ?? "",
  });

  const sortOptions = [
    { id: 0, name: "Name", label: "Name" },
    { id: 1, name: "Email", label: "Email" },
    { id: 2, name: "DateCreated", label: "Date created" },
    { id: 3, name: "PhoneNumber", label: "Phone number" },
  ];

  const sortDirections = [
    { id: 0, name: "asc", label: "Ascending" },
    { id: 1, name: "desc", label: "Descending" },
  ];

  useEffect(() => {
    fetchUsers(initialFilterValues);
  }, []);

  const fetchUsers = async (values: IFilter) => {
    await http_common
      .get<IPaginatedList<IUser>>(
        `api/Accounts/getByFilter?${convertFilterToQueryString(values)}`,
      )
      .then((resp: { data: SetStateAction<IPaginatedList<IUser>> }) => {
        setUsersPaginatedList(resp.data);
      })
      .catch((error) => {
        toast.error("Failed to fetch users data");
        console.error("Error fetching users data:", error);
      });
  };

  const handleDelete = async (id: number | string) => {
    await http_common
      .delete(`api/Accounts/${id}`)
      .then(async () => {
        toast.success("User deleted successfully");
        await fetchUsers(initialFilterValues);
      })
      .catch((error) => {
        toast.error("Failed to delete user");
        console.error("Error deleting user:", error);
      });
  };

  const { handleSearchChange } = useSearch({
    setInitialFilterValues,
    fetchData: fetchUsers,
  });

  const { handlePageChange } = usePage({
    setInitialFilterValues,
    fetchData: fetchUsers,
  });

  const { handleSortChange } = useSort({
    setInitialFilterValues,
    fetchData: fetchUsers,
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
            onSubmit={fetchUsers}
            enableReinitialize={true}
          >
            {({ handleChange, errors, touched, handleBlur }) => (
              <Form className="flex gap-5">
                <SearchGroup
                  onSearch={handleSearchChange}
                  label="Search Users"
                  value={initialFilterValues.searchTerm}
                ></SearchGroup>
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
                    Roles
                  </th>
                  <th className="px-4 sm:px-6 py-3 sm:py-4 font-medium text-gray-900 w-1/5">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 border-t border-gray-100">
                {usersPaginatedList.items.map((user) => (
                  <tr key={user.id} className="hover:bg-gray-50">
                    <th className="px-6 py-4 font-normal text-gray-900 ali w-1/5">
                      <div className="flex gap-3">
                        <div className="relative h-10 w-10">
                          <img
                            className="h-10 w-10 rounded-full object-cover object-center max-w-none mr-5"
                            src={`${http_common.getUri()}/images/300_${
                              user.image
                            }`}
                            alt=""
                          />
                        </div>
                        <div className="font-medium text-gray-700 flex items-center">
                          {user.firstName + " " + user.lastName}
                        </div>
                      </div>
                    </th>
                    <td className="px-6 py-4 w-1/5">{user.email}</td>
                    <td className="px-6 py-4 w-1/5">
                      {user.dateCreated
                        ? new Date(user.dateCreated).toLocaleDateString()
                        : "N/A"}
                    </td>
                    <td className="px-6 py-4 w-1/5">{user.phoneNumber}</td>
                    <td className="px-6 py-4 w-1/5">
                      <div className="flex flex-col gap-3">
                        {user.roles && user.roles.length > 0 ? (
                          user.roles.map((role, index) => (
                            <span
                              key={index}
                              className="flex justify-center rounded-full bg-blue-50 px-2 py-1 text-xs font-semibold text-blue-600"
                            >
                              {role}
                            </span>
                          ))
                        ) : (
                          <span>No roles</span>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 w-1/5">
                      <div className="flex justify-end gap-4">
                        <ModalDelete
                          id={user.id}
                          text={user.firstName + " " + user.lastName}
                          deleteFunc={handleDelete}
                        >
                          <button type="button">
                            <BsFillTrashFill size={20} />
                          </button>
                        </ModalDelete>
                        <button
                          type="button"
                          onClick={() => navigate(`update/${user.id}`)}
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
          onPageChange={handlePageChange}
          totalPages={usersPaginatedList.totalPages}
          currentPage={initialFilterValues.pageNumber}
        ></PaginationGroup>
      </div>
    </>
  );
}

export default UsersListPage;
