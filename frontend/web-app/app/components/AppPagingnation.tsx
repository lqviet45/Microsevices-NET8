'use client'

import { Pagination } from 'flowbite-react';
import React, { useState } from 'react';

type Props = {
    currentPage: number,
    pageCount: number,
    pageChanged: (page: number) => void;
}

export default function AppPagination({ currentPage, pageCount, pageChanged }: Props) {

    return (
        <Pagination
            currentPage={currentPage}
            onPageChange={e => pageChanged(e)}  
            totalPages={pageCount}
            layout='pagination' // pagination | pagination-compact | pagination-compact-center | pagination-compact-right | pagination-compact-left | pagination-compact-justify | pagination-compact-justify-center | pagination-compact-justify-right | pagination-compact-justify-left | pagination-compact-justify-justify
            showIcons={true} // true | false
            className='text-blue-500 mb-5'
        />
    );
}